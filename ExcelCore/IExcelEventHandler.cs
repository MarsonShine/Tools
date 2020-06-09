using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExcelCore {
    public interface IExcelEventHandler {
        Task Handle();
    }

    public interface IExcelEventHandler<TExcelEntity>
        where TExcelEntity : IExcelEntity {
            Task Handle();
        }
}