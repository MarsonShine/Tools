using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace FileExtractor
{
    public class HashFileCollection : ICollection<HashFile>
    {
        private readonly List<HashFile> publishHashFiles;
        
        public HashFileCollection()
        {
            publishHashFiles = new List<HashFile>();
        }

        public int Count => publishHashFiles.Count;

        public bool IsReadOnly => false;

        public void Add(HashFile item)
        {

        }

        public void Clear()
        {

        }

        public bool Contains(HashFile item)
        {
            return false;
        }

        public void CopyTo(HashFile[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<HashFile> GetEnumerator()
        {
            return publishHashFiles.GetEnumerator();
        }

        public bool Remove(HashFile item)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
