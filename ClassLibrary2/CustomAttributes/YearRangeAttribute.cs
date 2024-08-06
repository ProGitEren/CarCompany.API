using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.CustomAttributes
{
    public class YearRangeAttribute : ValidationAttribute
    {
        private readonly int _minimumYear;

        public YearRangeAttribute(int minimumuYear)
        {
            _minimumYear = minimumuYear;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is int year)
            { 
                    int currentYear = DateTime.Now.Year;
                if (year >= currentYear && year <= currentYear) 
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult($"The field {validationContext.DisplayName} must be between {_minimumYear} and {currentYear}.");
            }

            return new ValidationResult($"The field {validationContext.DisplayName} must be a valid year.   ");
        }

    }
}
