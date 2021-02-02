using Grpc.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoggerModule.Grpcs
{
    public class GrpcContextConvertor
    {
        public static ServerCallContextHttpContextLogEvent Convert2LogEvent(HttpContext context)
        {
            string requestIp = context.Connection.RemoteIpAddress.MapToIPv4().ToString() + ":" + context.Connection.RemotePort;
            string requestId = getHeaders(context, "requestId");
            string platformId = getHeaders(context, "platformId");
            string userflag = getHeaders(context, "userflag");
            //string duration = getHeaders(context, "duration");
            string url = requestIp + context.Request.Path.Value;

            static string getHeaders(HttpContext context, string key)
            {
                if (context == null) return "";
                if (context.Request.Headers.TryGetValue(key, out var val))
                    return val.ToString();
                else
                    return "";
            }

            return new ServerCallContextHttpContextLogEvent(requestIp, url, requestId, platformId, userflag);
        }

        public static ServerCallContextHttpContextLogEvent Convert2LogEvent(Metadata metadata)
        {
            string sourceIp = "", platformId = "", requestId = "", userflag = "", url = "";
            foreach (var entry in metadata)
            {
                if (entry.Key.Equals(ServerCallContextHttpContextLogEventConst.SourceIp, StringComparison.OrdinalIgnoreCase))
                {
                    sourceIp = entry.Value;
                }
                else if (Equals(entry.Key, ServerCallContextHttpContextLogEventConst.PlatformId))
                {
                    platformId = entry.Value;
                }
                else if (Equals(entry.Key, ServerCallContextHttpContextLogEventConst.RequestId))
                {
                    requestId = entry.Value;
                }
                else if (Equals(entry.Key, ServerCallContextHttpContextLogEventConst.UserFlag))
                {
                    userflag = entry.Value;
                }
                //else if (Equals(entry.Key, ServerCallContextHttpContextLogEventConst.Duration))
                //{
                //    duration = entry.Value;
                //}
                else if (Equals(entry.Key, ServerCallContextHttpContextLogEventConst.Url))
                {
                    url = entry.Value;
                }
            }
            return new ServerCallContextHttpContextLogEvent(sourceIp, url, requestId, platformId, userflag);
        }

        private static bool Equals(string str1, string str2) => string.Equals(str1, str2, StringComparison.OrdinalIgnoreCase);
    }
}
