using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace ExcelCore {
    public class ExcelHelper : IExcelExport, IExcelImport {
        private IWorkbook workbook;
        private ISheet sheet;
        private IRow title, rows;
        private int[] columnsIndex;
        private int sheetIndex = -1, titleRowIndex = -1, contentRowIndex = -1;
        public byte[] Export<T>(List<T> source, string fileName = "demo.xlsx", string sheetName = "sheet1") where T : IExcelEntity {
            workbook = new XSSFWorkbook();
            sheet = workbook.CreateSheet(sheetName);

            var sourceType = typeof(T);
            var properties = sourceType.GetProperties()
                .Where(p => p.CustomAttributes.Any())
                .OrderBy(p => p.GetCustomAttribute<ExcelColumnAttribute>().Order)
                .ToList();

            SetExcelTitle(properties);
            for (int i = 0; i < source.Count; i++) {

                rows = sheet.CreateRow(i + 1);
                SetExcelRowBody(source[i], properties);
            }

            using(var ms = new MemoryStream()) {
                workbook.Write(ms);
                ms.Flush();
                var buffer = ms.ToArray();

                return buffer;
            }
        }

        private void CheckNull<T>(List<T> source, string[] titleNames) {
            if (source == null || source.Count == 0) throw new ArgumentNullException(nameof(source));
            if (titleNames == null || titleNames.Length == 0) throw new ArgumentNullException(nameof(titleNames));
        }

        private void SetExcelTitle(List<PropertyInfo> props) {
            title = sheet.CreateRow(0);
            var attrs = props.Select(p => p.GetCustomAttribute<ExcelColumnAttribute>(false))
                .ToList();

            for (int i = 0; i < attrs.Count; i++) {
                title.CreateCell(i).SetCellValue(attrs[i].Name.Trim());
            }
        }

        public ExcelHelper OpenExcel(string path) {
            using var file = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
            if (path.EndsWith(".xls")) {
                workbook = new HSSFWorkbook(file);
            }
            workbook = new XSSFWorkbook(file);

            sheet = workbook.GetSheetAt(sheetIndex);
            rows = sheet.GetRow(contentRowIndex + 1);

            return this;
        }

        public DynamicExcelBuilder OpenExcel(Stream stream, List<IExcelEntity> source, List<ExcelOperationResultDescriptor> resultDescriptors) {
            using var fs = stream;
            var workbook = new XSSFWorkbook(fs);
            var sheet = workbook.GetSheetAt(sheetIndex);
            var rows = sheet.GetRow(contentRowIndex + 1);

            return new DynamicExcelBuilder(new ExcelContext(workbook, sheet, titleRowIndex, contentRowIndex, source, resultDescriptors));
        }

        public DynamicExcelBuilder LoadSource<T>(List<T> source, List<ExcelOperationResultDescriptor> resultDescriptors)
        where T : IExcelEntity {
            return new DynamicExcelBuilder(new ExcelContext(workbook, sheet, titleRowIndex, contentRowIndex, source.Cast<IExcelEntity>().ToList(), resultDescriptors));
        }

        private void SetExcelRowBody(object obj, List<PropertyInfo> properties) {
            var attrs = properties.Where(p => p.CustomAttributes.Any())
                .OrderBy(p => p.GetCustomAttribute<ExcelColumnAttribute>().Order)
                .ToList();

            for (int i = 0; i < attrs.Count; i++) {
                //object[] propertyValue = new object[attrs.Count];
                var val = attrs[i].GetValue(obj);
                rows.CreateCell(i).SetCellValue(val?.ToString());
            }

        }

        public ExcelHelper InitSheetIndex(int sheetIndex) {
            this.sheetIndex = sheetIndex;
            return this;
        }

        public ExcelHelper InitStartReadRowIndex(int titleRowIndex, int contentRowIndex) {
            this.titleRowIndex = titleRowIndex;
            this.contentRowIndex = contentRowIndex;
            return this;
        }

        private void AutoAnalyzeSheetIndex() {
            if (workbook == null) throw new ArgumentNullException("文件读取失败");
            var sheetCount = workbook.NumberOfSheets;
            if (sheetCount > 2) throw new ArgumentNullException("模板解析错误，请确认导入的模板格式");

            sheetIndex = sheetCount - 1;
        }

        public List<T> Import<T>(IFormFile file) {
            try {
                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);
                ms.Seek(0, SeekOrigin.Begin);
                if (file.FileName.EndsWith(".xls")) {
                    workbook = new HSSFWorkbook(ms);
                }
                workbook = workbook ?? new XSSFWorkbook(ms);

                if (sheetIndex == -1) {
                    AutoAnalyzeSheetIndex();
                }
                if (titleRowIndex == -1) {
                    throw new InvalidOperationException($"无效操作：请初始化 {nameof(titleRowIndex)} 与 {nameof(contentRowIndex)}，您在解析文件之前应调用方法 InitStartReadRowIndex");
                }

                sheet = workbook.GetSheetAt(sheetIndex);
                var propertys = typeof(T).GetProperties()
                    .Where(p => p.CustomAttributes.Any(p => p.AttributeType == typeof(ExcelColumnAttribute)))
                    .ToArray();

                var propertyDesc = propertys.Select(p => p.GetCustomAttribute<ExcelColumnAttribute>())
                    .ToArray();

                // 扫描 excel 的标题与 T 对象属性对应
                var titleRow = sheet.GetRow(titleRowIndex);
                // 初始化excel标题对应实体属性顺序索引的状态
                columnsIndex = new int[titleRow.LastCellNum];
                for (int i = 0; i < titleRow.LastCellNum; i++) {
                    columnsIndex[i] = -1;
                }
                for (int i = 0; i < titleRow.LastCellNum; i++) {
                    var cell = titleRow.GetCell(i);
                    if (cell == null) continue;
                    var excelTitle = cell.ToString().Trim();
                    columnsIndex[i] = propertyDesc.FindIndex(p => p.Name.Trim() == excelTitle);
                }

                var list = new List<T>();

                ReadCellValueAndFillToList(propertys, list);

                return list;
            } catch (Exception) {
                throw new Exception("模板解析错误，请确认导入的模板格式");
            }
        }

        private void ReadCellValueAndFillToList<T>(PropertyInfo[] propertys, IList<T> list) {
            var cellNum = sheet.GetRow(contentRowIndex);
            string value = null;

            //int num = cellNum.Cells.Count;
            for (int i = contentRowIndex; i <= sheet.LastRowNum; i++) {
                IRow row = sheet.GetRow(i);
                if (row == null) continue;
                var obj = Activator.CreateInstance<T>();
                for (int j = 0; j < columnsIndex.Length; j++) {
                    var propertyLocation = columnsIndex[j];
                    if (propertyLocation == -1) continue;

                    //空值忽略
                    if (row.GetCell(j) == null)
                        continue;
                    // cell 表达式
                    var cell = row.GetCell(j);
                    if (cell.CellType == CellType.Formula) {
                        cell.SetCellType(CellType.String);
                        value = cell.ToString();
                    } else {
                        value = cell.ToString();
                    }

                    string str = (propertys[propertyLocation].PropertyType).FullName;
                    if (str == "System.String") {
                        propertys[propertyLocation].SetValue(obj, value, null);
                    } else if (str == "System.DateTime") {
                        DateTime.TryParse(value, out DateTime pdt);
                        propertys[propertyLocation].SetValue(obj, pdt, null);
                    } else if (str == "System.Boolean") {
                        bool.TryParse(value, out bool pb);
                        propertys[propertyLocation].SetValue(obj, pb, null);
                    } else if (str == "System.Int16") {
                        short.TryParse(value, out short pi16);
                        propertys[propertyLocation].SetValue(obj, pi16, null);
                    } else if (str == "System.Single") {
                        float.TryParse(value, out float f);
                        propertys[propertyLocation].SetValue(obj, f, null);
                    } else if (str == "System.Double") {
                        double.TryParse(value, out double d);
                        propertys[propertyLocation].SetValue(obj, d, null);
                    } else if (str == "System.Int32") {
                        int.TryParse(value, out int pi32);
                        propertys[propertyLocation].SetValue(obj, pi32, null);
                    } else if (str == "System.Int64") {
                        long.TryParse(value, out long pi64);
                        propertys[propertyLocation].SetValue(obj, pi64, null);
                    } else if (str == "System.Byte") {
                        byte.TryParse(value, out byte pb);
                        propertys[propertyLocation].SetValue(obj, pb, null);
                    } else {
                        propertys[propertyLocation].SetValue(obj, null, null);
                    }
                }

                list.Add(obj);
            }
        }
    }
}