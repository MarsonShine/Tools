namespace ExcelCore
{
    using NPOI.SS.UserModel;
    using System;
    using System.Collections.Generic;

    public class ExcelContext
    {
        private ISheet _sheet;
        public IWorkbook Workbook { get; }
        public ISheet Sheet => _sheet ?? throw new ArgumentNullException(nameof(_sheet));
        public List<IExcelEntity> Entities { get; }
        public List<ExcelOperationResultDescriptor> ResultDescriptors { get; }
        public int ContentRowIndex { get; }
        public int TitleRowIndex { get; }

        public ExcelContext(IWorkbook workbook, ISheet sheet, int titleRowIndex, int contentRowIndex, List<IExcelEntity> entities, List<ExcelOperationResultDescriptor> resultDescriptors)
        {
            Workbook = workbook;
            _sheet = sheet;
            Entities = entities;
            ResultDescriptors = resultDescriptors;
            ContentRowIndex = contentRowIndex;
            TitleRowIndex = titleRowIndex;
        }

        public void SwitchSheet(int sheet)
        {
            _sheet = Workbook.GetSheetAt(sheet);
        }
    }
}