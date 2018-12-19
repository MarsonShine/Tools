using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelOperator
{
    public class Excel2007Operator : NpoiExcelOperator
    {
        private readonly string _filepath;

        public Excel2007Operator(string filepath)
        {
            _filepath = filepath;
        }
        public override DataTable ReadExcel(string filepath)
        {
            using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                XSSFWorkbook xssfworkbook = new XSSFWorkbook(fs);
                ISheet sheet = xssfworkbook.GetSheetAt(0);
                IRow header = sheet.GetRow(sheet.FirstRowNum);
                List<int> columns = new List<int>();
                DataTable dt = new DataTable();
                CalculateTool.ReadHeader(header, dt, columns);
                CalculateTool.ReadBody(sheet, dt, columns);
                return dt;
            }
        }

        public override void WriteTable(DataTable dt, string filepath)
        {
            XSSFWorkbook xssfworkbook = new XSSFWorkbook();
            ISheet sheet = xssfworkbook.CreateSheet(dt.TableName == "" ? FILE_NAME : dt.TableName);
            IRow header = sheet.CreateRow(0);
            CalculateTool.WriteHeader(header, dt);
            CalculateTool.WriteBody(sheet, dt);
            using (MemoryStream ms = new MemoryStream())
            {
                xssfworkbook.Write(ms);
                var buf = ms.ToArray();
                using (FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(buf, 0, buf.Length);
                    fs.Flush();
                }
            }
        }
    }
}
