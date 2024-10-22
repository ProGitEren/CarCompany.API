using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace Infrastucture.Extensions
{
    public static class ValidatorExtensions
    {
        public static List<string> stringErrors(this ValidationResult result) 
        {
            return result.Errors.Select(x => $"Property {x.PropertyName} failed validation. ErrorMessage = {x.ErrorMessage}").ToList();
        }
    }
}
