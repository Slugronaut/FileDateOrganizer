using System.IO;

namespace FDO.Model
{

    /// <summary>
    /// 
    /// </summary>
    public class FileOp
    {
        public enum Operations
        {
            Move,
            Copy,
        }

        public Operations Op { get; private set; }
        public FileBlob FileRef { get; private set; }
        public string DestPath { get; set; }


        public FileOp(Operations op, FileBlob blob, string dest)
        {
            Op = op;
            FileRef = blob;
            DestPath = dest;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>false if the destination path already has a file with the given filename of the FileBlob, true otherwise.</returns>
        public void Execute()
        {
            var srcPath = FileRef.FullSrcPath;
            var destPath = Path.Combine(DestPath, FileRef.DestFileName+FileRef.Extension);

            if (Op == Operations.Copy)
                File.Copy(srcPath, destPath, true);
            else if (Op == Operations.Move)
                File.Move(srcPath, destPath);
        }
    }
}
