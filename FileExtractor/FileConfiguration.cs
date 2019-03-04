using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileExtractor
{
    public class FileConfiguration
    {
        private static readonly IConfiguration configuration;
        static FileConfiguration() {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("publishConfigs.json",optional: true);
            configuration = builder.Build();
        }
        public static IConfiguration Configuration => configuration;
    }
}
