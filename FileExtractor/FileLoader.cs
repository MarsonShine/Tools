using Microsoft.Extensions.Configuration;

namespace FileExtractor
{
    public class FileLoader
    {
        public readonly FileConfiguration fileConfiguration;
        public FileLoader()
        {
            fileConfiguration = new FileConfiguration();
        }

        public void Load()
        {
            var historyPath = fileConfiguration.GetPublishConfig().HistoryFolderPath;

        }

        public IConfiguration Configuration = FileConfiguration.Configuration;

        public FileConfiguration FileConfiguration => fileConfiguration;
    }
}