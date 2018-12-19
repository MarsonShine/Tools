using System;
using System.IO;
using ExcelOperator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ToolsTest
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void Read_Table_IS_NotNull_From_Excel_Test()
        {
            string filepath = "回单复核记录.xlsx";
            var dt = ExcelHelper.ReadTable(Path.Combine(Directory.GetCurrentDirectory(), filepath));
            Assert.IsNotNull(dt);
        }
        [TestMethod]
        public void Read_Table_Rows_Equal_Test()
        {
            string filepath = "回单复核记录.xlsx";
            var dt = ExcelHelper.ReadTable(Path.Combine(Directory.GetCurrentDirectory(), filepath));
            Assert.AreEqual(1, dt.Rows.Count);
        }
        [TestMethod]
        public void Write_Excel_From_Table_Success()
        {
            string filepath = "回单复核记录.xlsx";
            var dt = ExcelHelper.ReadTable(Path.Combine(Directory.GetCurrentDirectory(), filepath));
            ExcelHelper.WriteExcel(dt, "下载回单.xls");
            bool Is_Writed_Success = false;
            Is_Writed_Success = File.Exists("下载回单.xls");
            Assert.AreEqual(Is_Writed_Success, true);
        }
    }
}
