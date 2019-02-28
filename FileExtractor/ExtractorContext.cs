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

        public ExtractorContext() {
            hashFileCollection = new HashFileCollection();
            hashFilePubilishContainer = new FilePublishTimeDictionary();
        }

        public FilePublishTimeDictionary Container => hashFilePubilishContainer;
        public HashFileCollection HashFileCollection => hashFileCollection;
    }
}
