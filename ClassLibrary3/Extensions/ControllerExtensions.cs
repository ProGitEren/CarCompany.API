using Infrastucture.Exceptions;
using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Extensions
{
    public static class ControllerExtensions
    {
        public static  IActionResult ToOk<TResponse>(this Result<TResponse> result)
        {
            return result.Match<IActionResult>(
               obj =>
               {
                   return new OkObjectResult(obj);
               },
               exception =>
               {
                   if (exception is HttpResponseException responseException)
                   {
                       return ((int)responseException.StatusCode) switch
                       {
                           400 => new BadRequestObjectResult(responseException.ToApiValidationErrorResponse()),
                           401 => new UnauthorizedObjectResult(responseException.ToApiException()),
                           404 => new NotFoundObjectResult(responseException.ToApiException()),
                           _ =>  new StatusCodeResult(500)
                       };
                   }
                   return new StatusCodeResult(500);
               });
        
        }

    }
}
