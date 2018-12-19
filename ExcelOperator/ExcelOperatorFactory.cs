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
        public static NpoiExcelOperator CreateExcelOperator(string filepath)
        {
            var excelType = GetExcelType(Path.GetExtension(filepath));
            switch (excelType)
            {
                case ExcelType.Excel2007:
                    return new Excel2007Operator(filepath);
                case ExcelType.Excel2003:
                default:
                    return new Excel2003Operator(filepath);
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
            return ExcelType.Excel2003;
        }
    }
}
