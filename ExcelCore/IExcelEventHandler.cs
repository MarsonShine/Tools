using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExcelCore
{
    public interface IExcelEventHandler
    {
        Task Handle();
    }
}
