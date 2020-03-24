using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using EnumConvert.EnumModel;

namespace EnumConvert
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            EnumScanner es = new EnumScanner();
            var dic = es.ToDescription(typeof(SexEnum));
            Console.WriteLine(JsonSerializer.Serialize(dic, new JsonSerializerOptions
            {
                WriteIndented = true
            }));

            var dics = es.Scan<SexEnum>();
            Console.WriteLine(JsonSerializer.Serialize(dics, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            }));

            Console.WriteLine(SexEnum.Female.ToDescription());
        }
    }
}
