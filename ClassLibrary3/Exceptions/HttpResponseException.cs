using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Exceptions
{
    public class HttpResponseException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public IEnumerable<string>? Errors { get; }


        public HttpResponseException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        public HttpResponseException(HttpStatusCode statusCode, IEnumerable<string> errors) 
        {
            StatusCode = statusCode;
            Errors = errors;
        }

    }
}
