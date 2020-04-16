using NPOI.SS.UserModel;
using System.Collections.Generic;

namespace ExcelCore
{
    public class ExcelContext
    {
        public IWorkbook Workbook { get; }
        public ISheet Sheet { get; }
        public List<IExcelEntity> Entities { get; }
        public List<ExcelOperationResultDescriptor> ResultDescriptors { get; }
        public int ContentRowIndex { get; }
        public int TitleRowIndex { get; }

        public ExcelContext(IWorkbook workbook, ISheet sheet, int titleRowIndex, int contentRowIndex, List<IExcelEntity> entities, List<ExcelOperationResultDescriptor> resultDescriptors)
        {
            Workbook = workbook;
            Sheet = sheet;
            Entities = entities;
            ResultDescriptors = resultDescriptors;
            ContentRowIndex = contentRowIndex;
            TitleRowIndex = titleRowIndex;
        }
    }
}