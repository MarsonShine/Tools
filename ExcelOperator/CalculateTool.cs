using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelOperator
{
    internal class CalculateTool
    {
        public static object GetCellValueType(ICell cell)
        {
            if (cell == null)
                return null;
            switch (cell.CellType)
            {
                case CellType.Unknown:
                    return null;
                case CellType.Numeric:
                    return cell.NumericCellValue;
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Blank:
                    return null;
                case CellType.Boolean:
                    return cell.BooleanCellValue;
                case CellType.Error:
                    return cell.ErrorCellValue;
                case CellType.Formula:
                default:
                    return "=" + cell.CellFormula;
            }
        }

        public static void ReadBody(ISheet sheet, DataTable dt, List<int> columns)
        {
            for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                DataRow dr = dt.NewRow();
                bool hasValue = false;
                foreach (int j in columns)
                {
                    dr[j] = CalculateTool.GetCellValueType(sheet.GetRow(i).GetCell(j));
                    if (dr[j] != null && dr[j].ToString() != string.Empty)
                    {
                        hasValue = true;
                    }
                }
                if (hasValue)
                {
                    dt.Rows.Add(dr);
                }
            }
        }

        public static void ReadHeader(IRow header, DataTable dt, List<int> columns)
        {
            for (int i = 0; i < header.LastCellNum; i++)
            {
                var cellValue = CalculateTool.GetCellValueType(header.GetCell(i));
                if (cellValue == null || cellValue.ToString() == string.Empty)
                {
                    dt.Columns.Add(new DataColumn("Columns" + i));
                }
                else
                {
                    dt.Columns.Add(new DataColumn(cellValue.ToString()));
                }
                columns.Add(i);
            }
        }

        public static void WriteHeader(IRow header, DataTable dt)
        {
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                ICell cell = header.CreateCell(i);
                cell.SetCellValue(dt.Columns[i].ColumnName);
            }
        }

        public static void WriteBody(ISheet sheet, DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row1 = sheet.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    ICell cell = row1.CreateCell(j);
                    SetCell(cell, dt.Rows[i][j].ToString(), dt.Columns[j].DataType);
                }
            }
        }

        private static void SetCell(ICell cell, string value, Type columnType)
        {
            switch (columnType.ToString())
            {
                case "System.String":
                    cell.SetCellValue(value);
                    cell.SetCellType(CellType.String);
                    break;
                case "System.DateTime":
                    DateTime timeV;
                    DateTime.TryParse(value, out timeV);
                    cell.SetCellValue(timeV);
                    break;
                case "System.Int16"://整型   
                case "System.Int32":
                case "System.Int64":
                    int intV = 0;
                    int.TryParse(value, out intV);
                    cell.SetCellValue(intV);
                    break;
                case "System.Decimal"://浮点型   
                case "System.Double":
                    double doubleV = 0;
                    double.TryParse(value, out doubleV);
                    cell.SetCellValue(doubleV);
                    break;
                case "System.DBNull":
                default:
                    cell.SetCellValue(value);
                    break;
            }
        }
    }
}
