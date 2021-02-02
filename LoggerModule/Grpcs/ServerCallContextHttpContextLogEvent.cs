using System;

namespace LoggerModule.Grpcs
{
    public class ServerCallContextHttpContextLogEvent
    {
        public ServerCallContextHttpContextLogEvent(string sourceIp, string url, string requestId, string platformId, string userflag)
        {
            SourceIp = sourceIp;
            Url = url;
            RequestId = requestId;
            PlatformId = platformId;
            UserFlag = userflag;
        }
        public string SourceIp { get; private set; }
        public string TargetIp { get; private set; }
        public string Host { get; private set; }
        public string Url { get; }
        public long Duration { get; private set; }
        public string Method { get; private set; }
        public string RequestId { get; }
        public string PlatformId { get; }
        public string UserFlag { get; }

        internal void Start()
        {
            Duration = DateTime.Now.Ticks;
        }

        internal double End()
        {
            return TimeSpan.FromTicks(DateTime.Now.Ticks - Duration).TotalMilliseconds;
        }
    }
}