using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace FileExtractor
{
    public class FileLoader
    {
        public readonly FileConfiguration fileConfiguration;
        private HashFileCollection historyFiles;
        public FileLoader()
        {
            fileConfiguration = new FileConfiguration();
        }

        public void Load()
        {
            var historyPath = fileConfiguration.GetPublishConfig().HistoryFolderPath;
            LoadDirectories(historyPath);   
        }

        private void LoadDirectories(string historyPath)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(historyPath);
            var dirs = directoryInfo.GetDirectories(historyPath);
            //取最新的文件夹
            var dir = dirs.OrderByDescending(p => p.CreationTimeUtc).FirstOrDefault();
            var file = dir.GetFiles().OrderByDescending(p => p.LastWriteTimeUtc).FirstOrDefault();
            if (file == null) return;
            ReadFile(file);
        }

        private void ReadFile(System.IO.FileInfo file)
        {
            using (var fileStream = file.OpenRead())
            {
                Span<byte> buffer = new Span<byte>();
                fileStream.Read(buffer);
                string content = GetFileContent(buffer);
                historyFiles = FileSerializer.Deserialize(content);
            }
        }

        private string GetFileContent(Span<byte> buffer)
        {
            return System.Text.Encoding.UTF8.GetString(buffer);
        }
        private string GetFileContent(byte[] buffer)
        {
            return System.Text.Encoding.UTF8.GetString(buffer);
        }
        public IConfiguration Configuration = FileConfiguration.Configuration;

        public FileConfiguration FileConfiguration => fileConfiguration;
        public HashFileCollection HistoryLastWriteFiles => historyFiles;
    }
}