using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Extensions
{
    public static class LoggerExtensions
    {
        public static string GetCorrelationId(this IHttpContextAccessor  _httpcontextAccessor)
        {
            return _httpcontextAccessor.HttpContext?.TraceIdentifier;
        }
    }
}
