using System.Collections.Generic;

namespace ExcelCore
{
    public interface IExcelExport
    {
         byte[] Export<T>(List<T> source, string fileName, string sheetName)
            where T : IExcelEntity;
    }
}