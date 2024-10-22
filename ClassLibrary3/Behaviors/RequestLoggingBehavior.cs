using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Behaviors
{
    public class RequestLoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
        where TRequest : IRequest<TResponse>
    
    {
        private readonly Serilog.ILogger _logger;

        public RequestLoggingBehavior(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.Information("Handling request: {RequestType}, Content: {@Request}, Timestamp: {Timestamp}",
              typeof(TRequest).Name, request, DateTime.UtcNow);

            TResponse response;
            try
            {
                // Pass the request to the next behavior or handler
                response = await next();
            }
            catch (Exception ex)
            {
                // Log the error in case of exceptions
                _logger.Error(ex, "Error occurred while handling request: {RequestType}, Content: {@Request}, Timestamp: {Timestamp}",
                    typeof(TRequest).Name, request, DateTime.UtcNow);
                throw;
            }

            // Log the response with structured data
            _logger.Information("Request handled: {RequestType}, Result: {@Response}, Timestamp: {Timestamp}",
                typeof(TRequest).Name, response, DateTime.UtcNow);

            return response;
        }
    }
}
