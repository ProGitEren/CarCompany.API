using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.CustomAttributes
{
    public class ValidCharactersAttribute : ValidationAttribute
    {
        private readonly char[] _allowedCharacters;

        public ValidCharactersAttribute(string allowedCharacters)
        {
            _allowedCharacters = allowedCharacters.ToCharArray();
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string input)
            {
                if (input.Length == 1 && Array.Exists(_allowedCharacters, c => c == input[0]))
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult($"The field {validationContext.DisplayName} must be a single character from allowed set.");
            }

            return new ValidationResult($"The field {validationContext.DisplayName} is not valid.");
        }
    }
}
