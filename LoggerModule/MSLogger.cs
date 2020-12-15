using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace LoggerModule
{
    public class MSLoggerEvent : ILogger, IReadOnlyList<KeyValuePair<string, object>>
    {
        readonly string _format;
        readonly object[] _parameters;
        IReadOnlyList<KeyValuePair<string, object>> _logValues;
        List<KeyValuePair<string, object>> _extraProperties;

        readonly IHttpContextAccessor _accessor;
        string categoryName;
        // 兼容 ILoggerProvider 模式
        public MSLoggerEvent(string categoryName, IHttpContextAccessor accessor)
        {
            this.categoryName = categoryName;
            _accessor = accessor;
        }

        public MSLoggerEvent(string format, params object[] values)
        {
            _format = format;
            _parameters = values;        
        }

        public MSLoggerEvent WithProperty(string name, object value)
        {
            var properties = _extraProperties ??= new List<KeyValuePair<string, object>>();
            if (!properties.Any(p => p.Key == name))
                properties.Add(new KeyValuePair<string, object>(name, value));
            return this;
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            if (MessagePropertyCount == 0)
            {
                if (ExtraPropertyCount > 0)
                    return _extraProperties.GetEnumerator();
                else
                    return Enumerable.Empty<KeyValuePair<string, object>>().GetEnumerator();
            }
            else
            {
                if (ExtraPropertyCount > 0)
                    return Enumerable.Concat(_extraProperties, LogValues).GetEnumerator();
                else
                    return LogValues.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public KeyValuePair<string, object> this[int index]
        {
            get
            {
                int extraCount = ExtraPropertyCount;
                if (index < extraCount)
                {
                    return _extraProperties[index];
                }
                else
                {
                    return LogValues[index - extraCount];
                }
            }
        }

        public int Count => MessagePropertyCount + ExtraPropertyCount;

        public override string ToString() => LogValues.ToString();

        private IReadOnlyList<KeyValuePair<string, object>> LogValues
        {
            get
            {
                if (_logValues == null)
                    LoggerExtensions.LogDebug(this, _format, _parameters);
                return _logValues;
            }
        }

        private int ExtraPropertyCount => _extraProperties?.Count ?? 0;

        private int MessagePropertyCount
        {
            get
            {
                if (LogValues.Count > 1 && !string.IsNullOrEmpty(LogValues[0].Key) && !char.IsDigit(LogValues[0].Key[0]))
                    return LogValues.Count;
                else
                    return 0;
            }
        }

        void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            _logValues = state as IReadOnlyList<KeyValuePair<string, object>> ?? Array.Empty<KeyValuePair<string, object>>();
        }

        bool ILogger.IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        IDisposable ILogger.BeginScope<TState>(TState state) => NullDisposable.Instance;


        public static Func<MSLoggerEvent, Exception, string> Formatter { get; } = (l, e) => l.LogValues.ToString();

        private class NullDisposable : IDisposable
        {
            public static readonly NullDisposable Instance = new NullDisposable();
            public void Dispose() { }
        }

    }



    public static class CustomLoggerExtensions
    {
        public static ILoggerFactory AddMSLogger(this ILoggerFactory factory, IHttpContextAccessor accessor)
        {
            factory.AddProvider(new MSLoggerProvider(accessor));
            return factory;
        }
    }
}
