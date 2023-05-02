using System;
using System.IO;

namespace FDO.Model
{
    public class FileBlob
    {
        readonly string delim = "_INC";
        public string FilePath { get; private set; }    //original source path without filename
        public string FileName { get; private set; }    //original source filename without extension
        public string RootFileName { get; private set; } //name of the file without any incremental numbers or extension
        public string Extension { get; private set; }

        int Increment = -1;
        public DateTime CreationDate;

        public string FullSrcPath => Path.Combine(FilePath, FileName) + Extension;        //original source path, filename and extension
        public string DestFileName
            {

                get
                {
                    string extraCrap = $"{delim}{Increment:d4}";
                    return $"{RootFileName}" + (Increment >= 0 ? extraCrap : string.Empty);       
                }
            }//destination filename derived from root filename, increment, and extension
        //public string FullDestPath => Path.Combine(FilePath, DestFileName)+Extension;   //dest path with incremental number and extension


        public FileBlob(string path, string file, DateTime date)
        {
            FilePath = path;
            CreationDate = date;

            #region Split for File Extension
            int extIndex = file.LastIndexOf('.');
            if (extIndex >= 0)
            {
                Extension = file.Substring(extIndex, file.Length - extIndex);
                FileName = file.Substring(0, extIndex);
            }
            else
            {
                Extension = string.Empty;
                FileName = file;
            }
            #endregion


            #region Split for Incremented Copies
            string[] split = FileName.Split(new[] { delim }, StringSplitOptions.None);
            if (split.Length < 2)
            {
                //looks like this is the first in the series
                Increment = -1;
                RootFileName = FileName;
            }
            else if (split.Length == 2)
            {
                if (!int.TryParse(split[1], out Increment))
                    throw new Exception($"There was a problem determining the incremental counter of '{FileName}'.");
                RootFileName = split[0];
            }
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        public void IncrementFileName()
        {
            Increment++;
        }
    }
}
