using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelOperator
{
    public static class ExcelExtensions
    {
        public static DataTable ToTable(this CsvReader csv)
        {
            DataTable dt = new DataTable();
            string[] headers = csv.GetFieldHeaders();
            foreach (string header in headers)
            {
                dt.Columns.Add(new DataColumn(header));
            }
            while (csv.ReadNextRecord())
            {
                DataRow dr = dt.NewRow();
                foreach (var name in headers)
                {
                    dr[name] = csv[name];
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}
