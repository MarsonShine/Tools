using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelOperator
{
    public class CsvOperator : ExcelOperator
    {
        public CsvOperator()
        {

        }
        public override DataTable ReadExcel(string filepath)
        {
            using (CsvReader csv = new CachedCsvReader(new StreamReader(filepath)))
            {
                return csv.ToTable();
            }
        }

        public override void WriteTable(DataTable dt, string filepath)
        {
            throw new NotImplementedException();
        }
    }
}
