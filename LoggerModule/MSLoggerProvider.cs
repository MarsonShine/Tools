﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace LoggerModule
{
    public class MSLoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, MSLoggerEvent> _loggers =
        new ConcurrentDictionary<string, MSLoggerEvent>();
        private readonly IHttpContextAccessor _accessor;

        public MSLoggerProvider(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public ILogger CreateLogger(string categoryName)
        {

            var logger = _loggers.GetOrAdd(categoryName, name => new MSLoggerEvent(name, _accessor));
            return logger;
        }

        public void Dispose() { }


    }
}
