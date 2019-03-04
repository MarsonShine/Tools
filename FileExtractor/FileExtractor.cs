using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileExtractor
{
    public class FileExtractor
    {
        private readonly FileFinder fileFinder;
        private readonly FileLoader fileLoader;
        public FileExtractor(FileFinder fileFinder,FileLoader fileLoader)
        {
            this.fileLoader = fileLoader;
            this.fileFinder = fileFinder;
        }

        public void Extractor()
        {
            FileComparer(fileFinder.Context.Container.Value, fileLoader.HistoryLastWriteFiles);
        }

        private void FileComparer(ConcurrentDictionary<int, FileInfo> fileInfoDictionary, HashFileCollection historyFiles)
        {
            foreach (var kp in fileInfoDictionary)
            {
                
            }
        }
    }
}
