using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace FileExtractor
{
    /// <summary>
    /// 找文件
    /// </summary>
    public class FileFinder
    {
        private const int parallelDegree = 10;
        private string[] specificFileTypes = new string[] { ".cs", ".aspx", ".ashx" };
        private const string searchPattern = "*";//(*.cs)|(*.aspx)
        private readonly ExtractorContext context;
        public FileFinder()
        {
            context = new ExtractorContext();
        }
        public HashFileCollection Start(string path)
        {
            var directories = Directory.GetDirectories(path);
            var directoryCount = directories.Length;
            Parallel.ForEach(directories, new ParallelOptions { MaxDegreeOfParallelism = 1 }, dir => Execute(dir));
            return Context.HashFileCollection;
        }

        private void Execute(string dir)
        {
            FindRootFile(dir);
        }

        private void FindRootFile(string dir)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(dir);
            foreach (var fileInfo in directoryInfo.GetFiles())
            {
                if (!specificFileTypes.Contains(fileInfo.Extension)) continue;
                var myFileInfo = new FileInfo(directoryInfo.ToString(), fileInfo.Name, fileInfo.LastWriteTimeUtc);
                Context.Container.Value.TryAdd(myFileInfo.ToHash(), myFileInfo);
            }
            FindDirectoriesRecursion(dir);
            
        }

        private void FindDirectoriesRecursion(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            var directories = directoryInfo.GetDirectories();
            foreach (var dir in directories)
            {
                foreach (var fileInfo in dir.GetFiles(searchPattern))
                {
                    if (!specificFileTypes.Contains(fileInfo.Extension)) continue;
                    var myFileInfo = new FileInfo(directoryInfo.ToString(), fileInfo.Name, fileInfo.LastWriteTimeUtc);
                    Context.Container.Value.TryAdd(myFileInfo.ToHash(), myFileInfo);
                } 
                FindDirectoriesRecursion(dir.FullName);
            }
        }

        public ExtractorContext Context => context;
    }
}
