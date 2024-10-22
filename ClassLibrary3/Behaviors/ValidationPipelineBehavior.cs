
using FluentValidation;
using Infrastucture.DTO.Dto_Users;
using Infrastucture.Exceptions;
using LanguageExt.ClassInstances;
using MediatR;
using Models.Abstractions;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LanguageExt.Common;
using ErrorOr;

namespace Infrastucture.Behaviors
{
    public class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>

    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // If there are no validators, pass the request to the next handler
            if (!_validators.Any())
            {
                return await next();
            }

            //Validation logic
            var context = new ValidationContext<TRequest>(request);
            var validationResults = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .Select(x => $"Invalid Property {x.PropertyName}. Error Message: {x.ErrorMessage}")
                .Distinct()
                .ToList();

            if (validationResults.Any())
            {
                // Handle the error response gracefully depending on the type of TResponse
                if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<bool>))
                {
                    // Handle cases like bool or other simple types
                    return (TResponse)(object)new Result<bool>(new HttpResponseException(System.Net.HttpStatusCode.BadRequest, validationResults));
                   
                }
                else if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
                {
                    var errorResponse = Activator.CreateInstance(
                       typeof(Result<>).MakeGenericType(typeof(TResponse).GetGenericArguments()[0]),
                       new HttpResponseException(System.Net.HttpStatusCode.BadRequest, validationResults)
                   );

                    return (TResponse)errorResponse!;
                }
            }





            // Pass the request to the next handler if validation succeeds
            return await next();
        }

       
    }
}
