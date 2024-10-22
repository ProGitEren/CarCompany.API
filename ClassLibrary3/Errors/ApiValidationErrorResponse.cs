using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace Infrastucture.Errors
{
    public class ApiValidationErrorResponse : BaseCommonResponse
    {
        //Asp.Net Core 8 Web API :https://www.youtube.com/watch?v=UqegTYn2aKE&list=PLazvcyckcBwitbcbYveMdXlw8mqoBDbTT&index=1

        public ApiValidationErrorResponse() : base(400)
        {

        }

        public ApiValidationErrorResponse(IEnumerable<ValidationFailure> errorList) : base(400) 
        {
            Errors = errorList.Select(x => $"Property: {x.PropertyName} failed validation. ErrorMessage : {x.ErrorMessage}");
        }

        public ApiValidationErrorResponse(IEnumerable<string> errorList) : base(400)
        {
            Errors = errorList;
        }

        public IEnumerable<string> Errors { get; set; }
    }
}
