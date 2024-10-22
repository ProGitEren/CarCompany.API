using Infrastucture.Errors;
using Infrastucture.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Extensions
{
    public static class ExceptionExtensions
    {

        public static ApiException ToApiException(this HttpResponseException ex) 
        {
            return new ApiException((int)ex.StatusCode, ex.Message);
        }

        public static ApiValidationErrorResponse ToApiValidationErrorResponse(this HttpResponseException ex)
        {
            return new ApiValidationErrorResponse(ex.Errors);
        }

    }
}
