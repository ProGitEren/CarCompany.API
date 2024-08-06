using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.CustomAttributes
{
    public class CustomAllowedValuesAttribute : ValidationAttribute
    {
        private readonly List<int> _allowedValues;

        public CustomAllowedValuesAttribute(params int[] allowedValues)
        {
            _allowedValues = allowedValues.ToList();

        }

        //public AllowedValuesAttribute(List<int> allowedValues)
        //{
        //    _allowedValues = allowedValues;            
        //}

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || !_allowedValues.Contains((int)value))
            {
                return new ValidationResult($"The field {validationContext.DisplayName} must be one of the following values: {string.Join(", ", _allowedValues)}.");
            }

            return ValidationResult.Success;
        }

    }
}
