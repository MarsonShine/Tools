using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExcelCore
{
    public class DynamicExcelBuilder
    {
        private readonly ExcelContext _excelContext;
        private readonly ExcelColorMap excelColorMap;

        private Dictionary<string, int> columnsIndex;
        private int[] buckets;
        public DynamicExcelBuilder(ExcelContext excelContext)
        {
            _excelContext = excelContext;
            SetExcelTitle();
            excelColorMap = new ExcelColorMap();
        }

        private void SetExcelTitle()
        {
            var titleRow = _excelContext.Sheet.GetRow(_excelContext.TitleRowIndex);
            // 初始化excel标题对应实体属性顺序索引的状态
            columnsIndex = new Dictionary<string, int>(titleRow.LastCellNum);

            // 按顺序度 excel title
            for (int i = 0; i < titleRow.LastCellNum; i++)
            {
                columnsIndex.Add(titleRow.GetCell(i).ToString().Trim(), i);

            }

            // 读取数据源对象的属性
            var sourceSingleEntity = _excelContext.Entities[0];
            var propertys = ReflectionHelper.GetProperties(sourceSingleEntity)
                .Where(p => p.CustomAttributes.Any(p => p.AttributeType == typeof(ExcelColumnAttribute)))
                .ToArray();
            var propertyDesc = propertys.Select(p => p.GetCustomAttribute<ExcelColumnAttribute>())
                .ToArray();

            buckets = new int[columnsIndex.Count];
            for (int i = 0; i < columnsIndex.Count; i++)
            {
                buckets[i] = -1;
            }
            var j = 0;
            foreach (var key in columnsIndex.Keys)
            {
                buckets[j++] = propertyDesc.FindIndex(p => p.Name == key);
            }

        }

        public DynamicExcelBuilder InsertRow(int startRowIndex, int insertRowCount)
        {
            var sources = _excelContext.Entities;
            _excelContext.Sheet.ShiftRows(startRowIndex, _excelContext.Sheet.LastRowNum + insertRowCount + 1, insertRowCount);

            for (int i = startRowIndex; i < startRowIndex + sources.Count; i++)
            {
                IRow rowSource = _excelContext.Sheet.GetRow(_excelContext.TitleRowIndex);
                IRow rowInsert = _excelContext.Sheet.CreateRow(i);
                rowInsert.Height = rowSource.Height;
                for (int colIndex = 0; colIndex < rowSource.LastCellNum; colIndex++)
                {
                    ICell cellSource = rowSource.GetCell(colIndex);
                    ICell cellInsert = rowInsert.CreateCell(colIndex);
                    if (cellSource != null)
                    {
                        cellInsert.CellStyle = cellSource.CellStyle;
                    }
                }
            }

            return this;
        }

        public DynamicExcelBuilder InsertCell(int rowIndex, int columnIndex, string columnValue, CellType cellType = CellType.String)
        {
            var sheet = _excelContext.Sheet;
            var titleRow = sheet.GetRow(rowIndex);
            var cell = titleRow.CreateCell(columnIndex, cellType);
            cell?.SetCellValue(columnValue);
            return this;
        }

        /// <summary>
        /// 追加列标题，标题是不允许重复的
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="columnValue"></param>
        /// <param name="cellType"></param>
        /// <param name="cellNumber"></param>
        /// <returns></returns>
        public DynamicExcelBuilder AppendTitleCell(int rowIndex, string columnValue, CellType cellType, out int cellNumber)
        {
            var sheet = _excelContext.Sheet;
            var titleIndex = _excelContext.TitleRowIndex;
            var row = sheet.GetRow(rowIndex);
            cellNumber = row.LastCellNum;
            if (IsDuplication(row, columnValue))
            {
                return this;
            }
            InsertCell(titleIndex, row.LastCellNum, columnValue, cellType);
            cellNumber++;
            return this;
        }

        private bool IsDuplication(IRow row, string columnValue)
        {
            var cells = row.Cells;
            return cells.Any(p => p.StringCellValue == columnValue);
        }

        public DynamicExcelBuilder AppendCellErrorValues(int startedRowIndex, int cellIndex)
        {
            var source = _excelContext.Entities;
            var result = _excelContext.ResultDescriptors;
            if (source.Count == 0) return this;
            if (result.Count == 0) return this;

            source.ForEach((item, index) =>
            {
                var row = _excelContext.Sheet.GetRow(startedRowIndex);
                if (row == null) throw new ExcelException($"目标行 {startedRowIndex} 不存在");
                var cell = row.GetCell(cellIndex);
                if (cell == null) cell = row.CreateCell(row.LastCellNum);
                if (!result[index].Success)
                {
                    cell.CellStyle = BuildCellStyle("Red");
                    cell.SetCellValue(result[index].ErrorMessage);
                }

            });
            return this;
        }

        public DynamicExcelBuilder InsertCellValue()
        {
            var contentRowIndex = _excelContext.ContentRowIndex;
            // 遍历行
            var source = _excelContext.Entities;
            var desc = _excelContext.ResultDescriptors;
            var properties = ReflectionHelper.GetProperties(source[0])
                .Where(p => p.CustomAttributes.Any(p => p.AttributeType == typeof(ExcelColumnAttribute)))
                .ToArray();
            var errorProps = typeof(IExcelError).GetProperties();

            for (int i = 0; i < _excelContext.Entities.Count; i++)
            {
                var rowInserting = _excelContext.Sheet.GetRow(contentRowIndex++);
                var entity = source[i];

                // 插入列
                for (int j = 0; j < rowInserting.LastCellNum; j++)
                {
                    if (buckets[j] == -1) continue;
                    var prop = properties[buckets[j]];

                    var cell = rowInserting.CreateCell(j);

                    var value = prop.GetValue(entity);
                    if (value is null) continue;

                    // 判断错误

                    if (errorProps.Any(p => p.Name == prop.Name))
                    {
                        var errorAttribute = prop.GetCustomAttribute<ExcelColumnAttribute>();
                        cell.CellStyle = BuildCellStyle(errorAttribute.FontColor);
                        cell.SetCellValue(desc[i].ErrorMessage);
                    }
                    else
                        cell.SetCellValue(value.ToString());
                }
            }

            return this;
        }

        private ICellStyle BuildCellStyle(string color)
        {
            var cellStyle = _excelContext.Workbook.CreateCellStyle();
            var font = _excelContext.Workbook.CreateFont();
            font.Color = excelColorMap.Colors[color];
            cellStyle.SetFont(font);
            return cellStyle;
        }

        public async Task<byte[]> WriteAsync()
        {
            using var stream = new MemoryStream();
            _excelContext.Workbook.Write(stream);
            var buffer = new byte[stream.Length];
            stream.Seek(0, SeekOrigin.Begin);
            await stream.ReadAsync(buffer, 0, buffer.Length);
            return buffer;
        }
    }
}
