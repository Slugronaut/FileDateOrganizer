using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FDO.Model
{
    public class FileCollector
    {
        /// <summary>
        /// Reads all files within a given directory and creates a collection of FileBlobs to represent them.
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public List<FileBlob> ReadCollectionFromFolder(string fullPath, bool recursive)
        {
            var list = new List<FileBlob>();
            Directory.Exists(fullPath);

            var files = Directory.GetFiles(fullPath, "*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                var dateTime = File.GetLastWriteTimeUtc(file);
                var pathSepIndex = file.LastIndexOf(Path.DirectorySeparatorChar);
                var path = file.Substring(0, pathSepIndex);
                var fileNameLen = (file.Length) - (pathSepIndex + 1);
                var fileName = file.Substring(pathSepIndex + 1, fileNameLen);

                list.Add(new FileBlob(path, fileName, dateTime));
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="recursive"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        public Task<List<FileBlob>> ReadCollectionFromFolderTask(string fullPath, bool recursive, CancellationToken cancelToken, Action<int> itemCountCallback = null)
        {
            return Task.Run(() =>
            {
                var list = new List<FileBlob>();
                Directory.Exists(fullPath);

                var files = Directory.GetFiles(fullPath, "*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                foreach (var file in files)
                {
                    if (cancelToken.IsCancellationRequested)
                        break;
                    var dateTime = File.GetLastWriteTimeUtc(file);
                    var pathSepIndex = file.LastIndexOf(Path.DirectorySeparatorChar);
                    var path = file.Substring(0, pathSepIndex);
                    var fileNameLen = (file.Length) - (pathSepIndex + 1);
                    var fileName = file.Substring(pathSepIndex + 1, fileNameLen);

                    list.Add(new FileBlob(path, fileName, dateTime));
                    itemCountCallback?.Invoke(list.Count);
                }
                return list;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="allFiles"></param>
        /// <returns></returns>
        public Dictionary<int, List<FileBlob>> CollectFilesByMonth(List<FileBlob> allFiles)
        {
            var col = new Dictionary<int, List<FileBlob>>();
            foreach (var blob in allFiles)
            {
                if (!col.TryGetValue(blob.CreationDate.Month, out List<FileBlob> list))
                    list = new List<FileBlob>();

                list.Add(blob);
                col[blob.CreationDate.Month] = list;
            }

            return col;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public void SanitizeDuplicateNames(Dictionary<int, List<FileBlob>> files)
        {
            List<string> fileNames = new List<string>(100);

            foreach (var key in files.Keys)
            {
                fileNames.Clear();
                foreach (var fileBlob in files[key])
                {
                    //loop over this filename till we find one that DOESN'T match.
                    while (fileNames.Contains(fileBlob.DestFileName))
                        fileBlob.IncrementFileName();

                    fileNames.Add(fileBlob.DestFileName);
                }
            }
        }

    }
}
