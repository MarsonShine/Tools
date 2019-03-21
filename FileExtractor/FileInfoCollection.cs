using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FileExtractor
{
    public class FileInfoCollection : ICollection<FileInfo>
    {
        private List<FileInfo> fileInfos;
        public FileInfoCollection() {
            fileInfos = new List<FileInfo>();
        }
        public int Count => fileInfos == null ? 0 : fileInfos.Count;

        public bool IsReadOnly => false;

        public void Add(FileInfo item)
        {
            if(!fileInfos.Contains(item))
                fileInfos.Add(item);
        }

        public void Clear()
        {
            fileInfos?.Clear();
        }

        public bool Contains(FileInfo item)
        {
            return fileInfos.Contains(item);
        }

        public void CopyTo(FileInfo[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<FileInfo> GetEnumerator()
        {
            return fileInfos?.GetEnumerator();
        }

        public bool Remove(FileInfo item)
        {
            if (fileInfos == null) return false;
            return fileInfos.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
