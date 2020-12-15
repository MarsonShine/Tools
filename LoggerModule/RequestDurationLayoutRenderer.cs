using NLog;
using NLog.Common;
using NLog.Config;
using NLog.LayoutRenderers;
using NLog.Web.LayoutRenderers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace LoggerModule
{
    [LayoutRenderer("aspnet-request-duration")]
    [ThreadSafe]
    public class RequestDurationLayoutRenderer : AspNetLayoutRendererBase
    {
        protected override void DoAppend(StringBuilder builder, LogEventInfo logEvent)
        {
            var context = HttpContextAccessor.HttpContext;
            var request = context?.Request;
            if (request == null)
                InternalLogger.Debug("HttpContext Request Lookup returned null");

            builder.Append(getRequestElasedTime());

            string getRequestElasedTime()
            {
                if (context != null)
                {
                    var r = context.Items.TryGetValue("ElapsedTime", out object val);
                    if (r)
                    {
                        var sw = (val as Stopwatch);
                        if (sw != null)
                        {
                            sw.Stop();
                        }
                        return sw.ElapsedMilliseconds + "ms";
                    }
                }
                return "";
            }
        }
    }
}
