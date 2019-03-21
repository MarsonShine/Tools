using System;
using System.Collections.Generic;
using System.Text;

namespace FileExtractor
{
    public class FileInfo
    {
        public FileInfo(string path,string name,DateTime time)
        {
            Path = path;
            Name = name;
            PublishTime = time;
        }
        public string Path { get; set; }
        public string Name { get; set; }
        public DateTime PublishTime { get; set; }

        public int ToHash()
        {
            return (Path + Name).GetHashCode();
        }
    }
}
