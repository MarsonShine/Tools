using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileExtractor
{
    public class FileConfiguration
    {
        private static readonly IConfiguration configuration = 
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("publishConfigs.json", optional: true)
                .Build();

        public static IConfiguration Configuration => configuration;

        public PublishConfig GetPublishConfig()
        {
            return configuration.GetSection("PublishConfig").Get<PublishConfig>();
        }
    }
}
