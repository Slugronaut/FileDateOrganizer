using Ookii.Dialogs.WinForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FDO.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class FileCopier
    {

        enum UniversalOperations
        {
            None,
            SkipAll,
            OverwriteAll,
            RenameAll,
            CancelAll,
        }

        enum DuplicateActions
        {
            Skipped,
            Writable,
            Cancelled,
        }


        readonly string RootPath;
        readonly FileOp.Operations Op;
        UniversalOperations UniversalOp;



        /// <summary>
        /// 
        /// </summary>
        /// <param name="op"></param>
        /// <param name="rootDestPath"></param>
        public FileCopier(FileOp.Operations op, string rootDestPath)
        {
            Op = op;
            RootPath = rootDestPath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="files"></param>
        public void PlaceFileBlobsInMonthlyFolders(Dictionary<int, List<FileBlob>> files)
        {
            UniversalOp = UniversalOperations.None;
            foreach(var key in files.Keys)
            {
                foreach (var fileBlob in files[key])
                {
                    var ym = new YearMonth(fileBlob.CreationDate);
                    var destPath = ConfirmDirectoryExists(ym.Name, RootPath);
                    FileOp op = new FileOp(Op, fileBlob, destPath);

                    var action = CheckForDuplicateFiles(op);
                    if (action == DuplicateActions.Cancelled)
                        break;
                    else if(action == DuplicateActions.Writable)
                        op.Execute();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="files"></param>
        public Task PlaceFileBlobsInMonthlyFolders(Dictionary<int, List<FileBlob>> files, CancellationToken cancelToken, Action<int> itemCountCallback = null)
        {
            return Task.Run( () =>
            {
                int count = 0;
                UniversalOp = UniversalOperations.None;
                foreach (var key in files.Keys)
                {
                    foreach (var fileBlob in files[key])
                    {
                        if (cancelToken.IsCancellationRequested)
                            return;

                        var ym = new YearMonth(fileBlob.CreationDate);
                        var destPath = ConfirmDirectoryExists(ym.Name, RootPath);
                        FileOp op = new FileOp(Op, fileBlob, destPath);

                        var action = CheckForDuplicateFiles(op);
                        if (action == DuplicateActions.Cancelled)
                            break;
                        else if (action == DuplicateActions.Writable)
                            op.Execute();
                        itemCountCallback?.Invoke(count++);
                    }
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        DuplicateActions CheckForDuplicateFiles(FileOp op)
        {
            //first, let's see if we even have a duplicate 'cause if we don't the rest of this can be skipped
            if (!File.Exists(Path.Combine(op.DestPath, op.FileRef.FileName + op.FileRef.Extension)))
                return DuplicateActions.Writable;

            //looks like we're screwed. there is a duplicate. what to do about it, though?
            var fileBlob = op.FileRef;
            switch (UniversalOp)
            {
                //HACK ALERT: UI-code in the business logic here!!
                case UniversalOperations.None:
                    {
                        //this has not happened before or if it has, the option to repeat a given operation has
                        ////not been selected. show the dialog asking the user what they want to do.
                        using (var dialog = new TaskDialog())
                        {
                            var skipButton = new TaskDialogButton("Skip");
                            var overwriteButton = new TaskDialogButton("Overwrite");
                            var renameButton = new TaskDialogButton("Automatically Rename");
                            var cancelButton = new TaskDialogButton("Cancel Task");
                            var doAllRadio = new TaskDialogRadioButton()
                            {
                                Text = "Do this for All Files",
                                Checked = false,
                            };

                            dialog.MainInstruction = $"The filename '{fileBlob.FileName}{fileBlob.Extension}' already exists in the folder '{op.DestPath}'.";
                            dialog.FooterIcon = TaskDialogIcon.Warning;

                            dialog.Buttons.Add(skipButton);
                            dialog.Buttons.Add(overwriteButton);
                            dialog.Buttons.Add(renameButton);
                            dialog.Buttons.Add(cancelButton);
                            dialog.RadioButtons.Add(doAllRadio);
                            var result = dialog.ShowDialog();


                            if (doAllRadio.Checked)
                            {
                                //if this is checked, we need to universally set this as our operation from here on out
                                if (result == skipButton)
                                {
                                    UniversalOp = UniversalOperations.SkipAll;
                                    return DuplicateActions.Skipped;
                                }
                                else if (result == overwriteButton)
                                {
                                    UniversalOp = UniversalOperations.OverwriteAll;
                                    return DuplicateActions.Writable;
                                }
                                else if (result == renameButton)
                                {
                                    while (IncrementedFileNameExists(op.DestPath, fileBlob))
                                        fileBlob.IncrementFileName();
                                    UniversalOp = UniversalOperations.RenameAll;
                                    return DuplicateActions.Writable;
                                }
                                else// (result == cancelButton)
                                {
                                    UniversalOp = UniversalOperations.CancelAll;
                                    return DuplicateActions.Cancelled;
                                }
                            }
                            else
                            {
                                if (result == skipButton)
                                    return DuplicateActions.Skipped;
                                else if (result == overwriteButton)
                                    return DuplicateActions.Writable;
                                else if (result == renameButton)
                                {
                                    while(IncrementedFileNameExists(op.DestPath, fileBlob))
                                        fileBlob.IncrementFileName();
                                    return DuplicateActions.Writable;
                                }
                                else// (result == cancelButton)
                                    return DuplicateActions.Cancelled;
                            }
                        }
                    }
                case UniversalOperations.SkipAll:
                    {
                        return DuplicateActions.Skipped;
                    }
                case UniversalOperations.OverwriteAll:
                    {
                        return DuplicateActions.Writable;
                    }
                case UniversalOperations.RenameAll:
                    {
                        while(IncrementedFileNameExists(op.DestPath, fileBlob))
                            fileBlob.IncrementFileName();
                        return DuplicateActions.Writable;
                    }
                case UniversalOperations.CancelAll:
                    {
                        return DuplicateActions.Cancelled;
                    }
                default:
                    {
                        return DuplicateActions.Cancelled;
                    }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileBlob"></param>
        /// <returns></returns>
        bool IncrementedFileNameExists(string destPath, FileBlob fileBlob)
        {
            var destFullPath = Path.Combine(destPath, fileBlob.DestFileName + fileBlob.Extension);
            return File.Exists(destFullPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="rootPath"></param>
        /// <returns></returns>
        public string ConfirmDirectoryExists(string folder, string rootPath)
        {
            string path = Path.Combine(rootPath, folder);
            if(!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="rootPath"></param>
        /// <returns></returns>
        public string ConfirmDirectoryExists(string fullPath)
        {
            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            return fullPath;
        }
    }
}
