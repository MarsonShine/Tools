using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelOperator
{
    public abstract class NpoiExcelOperator
    {
        protected const string FILE_NAME = "Column1";
        abstract public DataTable ReadExcel(string filepath);
        abstract public void WriteTable(DataTable dt, string filepath);
    }
}
