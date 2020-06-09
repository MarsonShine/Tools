using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelCore {
    public interface IExportExcelEventHandler : IExcelEventHandler {
        IList<IExcelEntity> Sources { get; }
        IList<ExcelOperationResultDescriptor> Results { get; }
    }

    public interface IExportExcelEventHandler<TExcelEntity> : IExcelEventHandler<TExcelEntity>
        where TExcelEntity : IExcelEntity {
            IList<TExcelEntity> Sources { get; }
            IList<ExcelOperationResultDescriptor> Results { get; }
        }
}