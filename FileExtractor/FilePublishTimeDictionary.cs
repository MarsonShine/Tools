using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace FileExtractor
{
    public class FilePublishTimeDictionary
    {
        private readonly ConcurrentDictionary<int, FileInfo> fileInfoContainer;
        public FilePublishTimeDictionary()
        {
            fileInfoContainer = new ConcurrentDictionary<int, FileInfo>();
        }

        public ConcurrentDictionary<int, FileInfo> Value => fileInfoContainer;
    }
}
