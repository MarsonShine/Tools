using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ExcelCore
{
    public interface IExcelImport
    {
         List<T> Import<T>(IFormFile file);
    }
}