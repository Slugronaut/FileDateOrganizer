using Ookii.Dialogs.WinForms;
using FDO.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Linq;
using Ookii;
using System.Threading.Tasks;

namespace FDO
{
    public partial class Form1 : Form
    {

        Dictionary<int, List<FileBlob>> BucketList = new Dictionary<int, List<FileBlob>>();
        ProgressDialog ProgressDiag;


        public Form1()
        {
            InitializeComponent();
        }

        private void buttonPerformOp_Click(object sender, EventArgs e)
        {
            CollectFiles();
        }

        private void buttonSetSrc_Click(object sender, EventArgs e)
        {
            using (var dialog = new VistaFolderBrowserDialog())
            {
                dialog.Description = "Select Source Folder";
                dialog.SelectedPath = textSrc.Text;
                dialog.ShowDialog(this);
                textSrc.Text = dialog.SelectedPath;
            }
        }

        private void buttonSetDest_Click(object sender, EventArgs e)
        {
            using (var dialog = new VistaFolderBrowserDialog())
            {
                dialog.Description = "Select Destination Folder";
                dialog.SelectedPath = textSrc.Text;
                dialog.ShowDialog(this);
                textDest.Text = dialog.SelectedPath;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void CollectFiles()
        {

            if (!ValidateFolders())
                return;
            BucketList.Clear();

            void workHandler(object s, DoWorkEventArgs args)
            {
                var progressDiag = s as ProgressDialog;
                FileCollector collector = new FileCollector();
                CancellationTokenSource cancelSource = new CancellationTokenSource();
                try
                {
                    var task = collector.ReadCollectionFromFolderTask(textSrc.Text, checkBoxRecursive.Checked, cancelSource.Token, (x) =>
                    {
                        //yeah baby! super-nesting of functions!!
                        progressDiag.ReportProgress(0, "Collecting file info from source directory...", $"{x} files found.");
                    });

                    Thread pollingThread = new Thread(new ThreadStart( () => {
                        while (true)
                        {
                            if (progressDiag.CancellationPending)
                            {
                                cancelSource.Cancel();
                                break;
                            }
                            Thread.Sleep(1);
                        }
                    }));
                    pollingThread.Start();
                    
                    //wait for result while not blocking UI somehow magically
                    task.Wait(cancelSource.Token);
                    pollingThread.Abort();

                    List<FileBlob> rawCol = task.Result;
                    BucketList = collector.CollectFilesByMonth(rawCol);
                    collector.SanitizeDuplicateNames(BucketList);
                }
                catch(OperationCanceledException ce)
                {
                    this.Invoke(new Action(() => MessageBox.Show(this, ce.Message)));
                }
                catch (Exception e)
                {
                    this.Invoke(new Action(() => MessageBox.Show(this, "A problem occurred while collecting files.\n\n" + e.Message)));
                }
                finally
                {
                    cancelSource.Dispose();
                    cancelSource = null;
                }
            }


            ProgressDiag = new ProgressDialog();
            var progress = ProgressDiag;

            progress.ProgressBarStyle = Ookii.Dialogs.WinForms.ProgressBarStyle.MarqueeProgressBar;
            progress.WindowTitle = "Working...";
            progress.Text = "Collecting file info from source directory...";
            progress.DoWork += workHandler;
            progress.RunWorkerCompleted += HandleCollectionComplete;
            progress.ShowCancelButton = true;
            progress.CancellationText = "Cancel";
            progress.ShowDialog(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="workArgs"></param>
        void HandleCollectionComplete(object s, RunWorkerCompletedEventArgs workArgs)
        {
            ProgressDiag.Dispose();
            ProgressDiag = null;
            if (MessageBox.Show(this, $"A total of {BucketList.Sum(x => x.Value.Count)} files were found.\n\nDo you want to process them now?", "Files Found", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                //invoke so that it's performed on the UI thread
                this.Invoke( new Action(() => ProcessFiles(BucketList)));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void ProcessFiles(Dictionary<int, List<FileBlob>> files)
        {
            if (!ValidateFolders()) return;
            var op = (FileOp.Operations)Enum.Parse(typeof(FileOp.Operations), this.comboOperation.Text);
            var cancelSource = new CancellationTokenSource();
            FileCopier fc = new FileCopier(op, textDest.Text);
            fc.ConfirmDirectoryExists(textDest.Text);
            var sumOfFiles = BucketList.Sum(x => x.Value.Count);

            void workHandler(object s, DoWorkEventArgs args)
            {
                var progressDiag = s as ProgressDialog;
                try
                {
                    var task = fc.PlaceFileBlobsInMonthlyFolders(files, cancelSource.Token, (x) =>
                    {
                        double percent = (double)(x * 100) / sumOfFiles;
                        progressDiag.ReportProgress((int)Math.Floor(percent), "Collecting file info from source directory...", $"{x} files processed.");
                    });

                    Thread pollingThread = new Thread(new ThreadStart(() => {
                        while (true)
                        {
                            if (progressDiag.CancellationPending)
                            {
                                cancelSource.Cancel();
                                break;
                            }
                            Thread.Sleep(1);
                        }
                    }));
                    pollingThread.Start();

                    task.Wait();
                    pollingThread.Abort();
                }
                catch(OperationCanceledException ce)
                {
                    this.Invoke(new Action(() => MessageBox.Show(this, ce.Message)));
                }
                catch (Exception e)
                {
                    this.Invoke(new Action(() => MessageBox.Show(this, "A problem occurred while processing files.\n\n" + e.Message)));
                }
                finally
                {
                    cancelSource.Dispose();
                    cancelSource = null;
                }
            }


            ProgressDiag = new ProgressDialog();
            var progress = ProgressDiag;

            progress.ProgressBarStyle = Ookii.Dialogs.WinForms.ProgressBarStyle.ProgressBar;
            progress.WindowTitle = "Working...";
            progress.Text = "Moving files to destination...";
            progress.DoWork += workHandler;
            progress.RunWorkerCompleted += HandleProcessComplete;
            progress.ShowCancelButton = true;
            progress.CancellationText = "Cancel";
            progress.ShowDialog(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="workArgs"></param>
        void HandleProcessComplete(object s, RunWorkerCompletedEventArgs workArgs)
        {
            ProgressDiag.Dispose();
            ProgressDiag = null;
            MessageBox.Show(this, "All done!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool ValidateFolders()
        {
            string src = textSrc.Text;
            string dest = textDest.Text;

            if (string.IsNullOrEmpty(src))
            {
                MessageBox.Show(this, "Please select a valid source folder, first.");
                return false;
            }

            if (!Directory.Exists(src))
            {
                MessageBox.Show(this, "The source folder is no longer valid. Have you renamed or removed it?");
                return false;
            }

            if (string.IsNullOrEmpty(dest))
            {
                MessageBox.Show(this, "Please select a valid destination folder, first.");
                return false;
            }

            if (!Directory.Exists(dest))
            {
                MessageBox.Show(this, "The destination folder is no longer valid. Have you renamed or removed it?");
                return false;
            }

            //is the dest inside of src? do not allow this
            if (dest.Contains(src))
            {
                MessageBox.Show(this, "The destination folder is inside of the source folder. This is not allowed.");
                return false;
            }

            //if the dest folder empty?
            IEnumerable<string> files = Directory.EnumerateFileSystemEntries(dest);
            using (IEnumerator<string> enumerator = files.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    return MessageBox.Show(this, "There are already files in the destination folder. Do you want to proceed anyway?", "Destination Not Empty", MessageBoxButtons.YesNo) == DialogResult.Yes;
                }
            }
            return true;
        }

        
    }
}
