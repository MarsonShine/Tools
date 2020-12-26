using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LoggerModule.Middwares
{
    public interface IPlatformMiddleware
    {
        Task InvokeAsync(HttpContext context);
    }
}
