using System;
using System.Collections.Generic;
using System.Text;

namespace FileExtractor
{
    //文件比较器，用来比较上次发布的时间与这次最新的文件修改时间
    public class FileComparer : IComparer<HashFile>
    {
        public int Compare(HashFile x,HashFile y)
        {
            return (x.PubliseTime - y.PubliseTime).Seconds;
        }
    }
}
