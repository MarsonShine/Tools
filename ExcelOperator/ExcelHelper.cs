using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelOperator
{
    public class ExcelHelper
    {
        public static DataTable ReadTable(string filepath)
        {
            var excelOperator = ExcelOperatorFactory.CreateExcelOperator(filepath);
            return excelOperator.ReadExcel(filepath);
        }

        public static void WriteExcel(DataTable dt, string filepath)
        {
            var excelOperator = ExcelOperatorFactory.CreateExcelOperator(filepath);
            excelOperator.WriteTable(dt, filepath);
        }

        public static void WriteExcel<T>(List<T> resource, string filepath) {
            var excelOperator = new AbstractDataExportBridge();
            excelOperator.WriteData(resource);
        }
    }
}
