using System;
using System.Collections.Generic;
using System.Text;

namespace LoggerModule.Configs
{
    public class LoggerConfig
    {
        public string PathFileName { get; set; } = "logs/log-{Hour}.log";
        public string MessageTemplate { get; set; } = "[{RequestId}] [{Timestamp:HH:mm:ss} [{AppRequestId} {PlatformId} {UserFlag}] {Level:u3}] {Message:lj} {NewLine}{Exception}";
        public int FileSizeLimit { get; set; } = 1073741824; // 1Gb
    }
}
