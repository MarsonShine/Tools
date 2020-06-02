using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelCore
{
    public class ExcelException : Exception
    {
        public ExcelException() : base() { }

        public ExcelException(string message) : base(message) { }
        public ExcelException(string message, Exception innerException) : base(message, innerException) { }
    }
}
