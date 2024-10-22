using Serilog.Context;

namespace WebAPI.MiddleWare
{
    public class RequestLogContextMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLogContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task InvokeAsync(HttpContext context) 
        {
            using (LogContext.PushProperty("Correlation Id", context.TraceIdentifier)) 
            {
                return _next(context);
            }
        
        }
    }
}
