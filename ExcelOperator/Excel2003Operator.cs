using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelOperator
{
    public class Excel2003Operator : NpoiExcelOperator
    {
        private readonly string _filepath;

        public Excel2003Operator(string filepath)
        {
            _filepath = filepath;
        }

        public override DataTable ReadExcel(string filepath)
        {
            if (string.IsNullOrEmpty(filepath))
                throw new ArgumentNullException(nameof(filepath));
            using (FileStream fs = new FileStream(_filepath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = WorkbookFactory.Create(fs);
                DataTable dt = new DataTable();
                ISheet sheet = workbook.GetSheetAt(0);
                IRow header = sheet.GetRow(sheet.FirstRowNum);
                List<int> columns = new List<int>();
                CalculateTool.ReadHeader(header, dt, columns);
                CalculateTool.ReadBody(sheet, dt, columns);
                return dt;
            }
        }

        public override void WriteTable(DataTable dt, string filepath)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            ISheet sheet = hssfworkbook.CreateSheet(dt.TableName == "" ? FILE_NAME : dt.TableName);
            IRow header = sheet.CreateRow(0);
            CalculateTool.WriteHeader(header, dt);
            CalculateTool.WriteBody(sheet, dt);
            using (MemoryStream stream = new MemoryStream())
            {
                hssfworkbook.Write(stream);
                var buf = stream.ToArray();
                using (FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(buf, 0, buf.Length);
                    fs.Flush();
                }
            }
        }
    }
}
