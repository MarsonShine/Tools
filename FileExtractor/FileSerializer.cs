using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileExtractor
{
    public class FileSerializer
    {
        public static HashFileCollection Deserialize(string content)
        {
            try
            {
                var files = JsonConvert.DeserializeObject<HashFileCollection>(content);
                return files;
            }
            catch
            {
                throw;
            }
            
        }
    }
}
