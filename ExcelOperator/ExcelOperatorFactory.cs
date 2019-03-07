using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelOperator
{
    public class ExcelOperatorFactory
    {
        public static ExcelOperator CreateExcelOperator(string filepath)
        {
            var excelType = GetExcelType(Path.GetExtension(filepath));
            switch (excelType)
            {
                case ExcelType.Excel2007:
                    return new Excel2007Operator(filepath);
                case ExcelType.Excel2003:
                    return new Excel2003Operator(filepath);
                case ExcelType.Csv:
                    return new CsvOperator();
                default:
                    return new Excel2007Operator(filepath);
            }
        }

        private static ExcelType GetExcelType(string fileExtension)
        {
            if (string.Compare(fileExtension, ".xls", ignoreCase: true) == 0)
            {
                return ExcelType.Excel2003;
            }
            if (string.Compare(fileExtension, ".xlsx", ignoreCase: true) == 0)
            {
                return ExcelType.Excel2007;
            }
            if (string.Compare(fileExtension, ".csv", ignoreCase: true) == 0)
                return ExcelType.Csv;
            return ExcelType.Excel2007;
        }
    }
}
