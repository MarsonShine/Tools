using NLog;
using NLog.Config;
using NLog.LayoutRenderers;
using NLog.Web.LayoutRenderers;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoggerModule.LayoutRenderers
{
    [LayoutRenderer("hours")]
    [ThreadSafe]
    public class HoursLayoutRenderer : AspNetLayoutRendererBase
    {
        protected override void DoAppend(StringBuilder builder, LogEventInfo logEvent)
        {
            builder.Append(DateTimeOffset.Now.Hour);
        }
    }
    [LayoutRenderer("year")]
    [ThreadSafe]
    public class YearLayoutRenderer : AspNetLayoutRendererBase
    {
        protected override void DoAppend(StringBuilder builder, LogEventInfo logEvent)
        {
            builder.Append(DateTimeOffset.Now.Year);
        }
    }

    [LayoutRenderer("month")]
    [ThreadSafe]
    public class MonthLayoutRenderer : AspNetLayoutRendererBase
    {
        protected override void DoAppend(StringBuilder builder, LogEventInfo logEvent)
        {
            builder.Append(DateTimeOffset.Now.Month);
        }
    }
}
