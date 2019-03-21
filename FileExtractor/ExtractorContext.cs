using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace FileExtractor
{
    public class ExtractorContext
    {
        private readonly FilePublishTimeDictionary hashFilePubilishContainer;
        private readonly HashFileCollection hashFileCollection;
        private readonly FileInfoCollection fileInfoCollection;

        public ExtractorContext()
        {
            hashFileCollection = new HashFileCollection();
            hashFilePubilishContainer = new FilePublishTimeDictionary();
            fileInfoCollection = new FileInfoCollection();
        }

        public FilePublishTimeDictionary Container => hashFilePubilishContainer;
        public HashFileCollection HashFileCollection => hashFileCollection;
        public FileInfoCollection FileInfoCollection => fileInfoCollection;
    }
}
