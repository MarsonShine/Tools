using Microsoft.AspNetCore.Http;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ExcelCore
{
    public class ExcelHelper : IExcelExport, IExcelImport
    {
        private IWorkbook workbook;
        private ISheet sheet;
        private IRow title, rows;
        private int[] columnsIndex;
        private int sheetIndex, titleRowIndex, contentRowIndex;

        public byte[] Export<T>(List<T> source, string fileName = "demo.xlsx", string sheetName = "sheet1") where T : IExcelEntity
        {
            workbook = new XSSFWorkbook();
            sheet = workbook.CreateSheet(sheetName);

            var sourceType = typeof(T);
            var properties = sourceType.GetProperties()
                .Where(p => p.CustomAttributes.Any())
                .OrderBy(p => p.GetCustomAttribute<ExcelColumnAttribute>().Order)
                .ToList();

            SetExcelTitle(properties);
            for (int i = 0; i < source.Count; i++)
            {

                rows = sheet.CreateRow(i + 1);
                SetExcelRowBody(source[i], properties);
            }

            using (var ms = new MemoryStream())
            {
                workbook.Write(ms);
                ms.Flush();
                var buffer = ms.ToArray();

                return buffer;
            }
        }

        private void CheckNull<T>(List<T> source, string[] titleNames)
        {
            if (source == null || source.Count == 0) throw new ArgumentNullException(nameof(source));
            if (titleNames == null || titleNames.Length == 0) throw new ArgumentNullException(nameof(titleNames));
        }

        private void SetExcelTitle(List<PropertyInfo> props)
        {
            title = sheet.CreateRow(0);
            var attrs = props.Select(p => p.GetCustomAttribute<ExcelColumnAttribute>(false))
                .ToList();


            for (int i = 0; i < attrs.Count; i++)
            {
                title.CreateCell(i).SetCellValue(attrs[i].Name);
            }
        }

        public ExcelHelper OpenExcel(string path)
        {
            using var file = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
            if (path.EndsWith(".xls"))
            {
                workbook = new HSSFWorkbook(file);
            }
            workbook = new XSSFWorkbook(file);

            sheet = workbook.GetSheetAt(sheetIndex);
            rows = sheet.GetRow(contentRowIndex + 1);

            return this;
        }

        public DynamicExcelBuilder LoadSource<T>(List<T> source, List<ExcelOperationResultDescriptor> resultDescriptors)
            where T : IExcelEntity
        {
            return new DynamicExcelBuilder(new ExcelContext(workbook, sheet, titleRowIndex, contentRowIndex, source.Cast<IExcelEntity>().ToList(), resultDescriptors));
        }

        private void SetExcelRowBody(object obj, List<PropertyInfo> properties)
        {
            var attrs = properties.Where(p => p.CustomAttributes.Any())
                .OrderBy(p => p.GetCustomAttribute<ExcelColumnAttribute>().Order)
                .ToList();



            for (int i = 0; i < attrs.Count; i++)
            {
                //object[] propertyValue = new object[attrs.Count];
                var val = attrs[i].GetValue(obj);
                rows.CreateCell(i).SetCellValue(val?.ToString());
            }

        }

        public List<T> Import<T>(IFormFile file)
        {
            MemoryStream ms = new MemoryStream();
            file.CopyTo(ms);
            ms.Seek(0, SeekOrigin.Begin);
            if (file.FileName.EndsWith(".xls"))
            {
                workbook = new HSSFWorkbook(ms);
            }
            workbook = workbook ?? new XSSFWorkbook(ms);
            sheet = workbook.GetSheetAt(0);
            var propertys = typeof(T).GetProperties()
                .Where(p => p.CustomAttributes.Any(p => p.AttributeType == typeof(ExcelColumnAttribute)))
                .ToArray();

            var propertyDesc = propertys.Select(p => p.GetCustomAttribute<ExcelColumnAttribute>())
                .ToArray();

            // 扫描 excel 的标题与 T 对象属性对应
            var titleRow = sheet.GetRow(0);
            // 初始化excel标题对应实体属性顺序索引的状态
            columnsIndex = new int[titleRow.LastCellNum];
            for (int i = 0; i < titleRow.LastCellNum; i++)
            {
                columnsIndex[i] = -1;
            }
            for (int i = 0; i < titleRow.LastCellNum; i++)
            {
                var excelTitle = titleRow.GetCell(i).ToString();
                columnsIndex[i] = propertyDesc.FindIndex(p => p.Name == excelTitle);
            }

            var list = new List<T>();

            ReadCellValueAndFillToList(propertys, list);

            return list;
        }

        public ExcelHelper InitSheetIndex(int sheetIndex)
        {
            this.sheetIndex = sheetIndex;
            return this;
        }

        public ExcelHelper InitStartReadRowIndex(int titleRowIndex, int contentRowIndex)
        {
            this.titleRowIndex = titleRowIndex;
            this.contentRowIndex = contentRowIndex;
            return this;
        }

        private void ReadCellValueAndFillToList<T>(PropertyInfo[] propertys, IList<T> list)
        {
            var cellNum = sheet.GetRow(1);
            string value = null;

            int num = cellNum.LastCellNum;
            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                var obj = Activator.CreateInstance<T>();
                for (int j = 0; j < num; j++)
                {
                    var propertyLocation = columnsIndex[j];
                    if (propertyLocation == -1) continue;

                    value = row.GetCell(j).ToString();
                    string str = (propertys[propertyLocation].PropertyType).FullName;
                    if (str == "System.String")
                    {
                        propertys[propertyLocation].SetValue(obj, value, null);
                    }
                    else if (str == "System.DateTime")
                    {
                        DateTime pdt = Convert.ToDateTime(value, CultureInfo.InvariantCulture);
                        propertys[propertyLocation].SetValue(obj, pdt, null);
                    }
                    else if (str == "System.Boolean")
                    {
                        bool pb = Convert.ToBoolean(value);
                        propertys[propertyLocation].SetValue(obj, pb, null);
                    }
                    else if (str == "System.Int16")
                    {
                        short pi16 = Convert.ToInt16(value);
                        propertys[propertyLocation].SetValue(obj, pi16, null);
                    }
                    else if (str == "System.Int32")
                    {
                        int pi32 = Convert.ToInt32(value);
                        propertys[propertyLocation].SetValue(obj, pi32, null);
                    }
                    else if (str == "System.Int64")
                    {
                        long pi64 = Convert.ToInt64(value);
                        propertys[propertyLocation].SetValue(obj, pi64, null);
                    }
                    else if (str == "System.Byte")
                    {
                        byte pb = Convert.ToByte(value);
                        propertys[propertyLocation].SetValue(obj, pb, null);
                    }
                    else
                    {
                        propertys[propertyLocation].SetValue(obj, null, null);
                    }
                }

                list.Add(obj);
            }
        }
    }
}