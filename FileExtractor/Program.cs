using System;

namespace FileExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            string wwwrootPath = Console.ReadLine();
            if (string.IsNullOrEmpty(wwwrootPath)) return;
            FileFinder finder = new FileFinder();
            finder.Start(wwwrootPath);
            
        }
    }
}
