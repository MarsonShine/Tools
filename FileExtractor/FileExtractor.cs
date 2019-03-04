using System;
using System.Collections.Generic;
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


    }
}
