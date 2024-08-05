using ClassLibrary2.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;


namespace WebAPI.Validation
{


    public class PasswordValidation
    {
        public class PasswordValidationResult
        {
            public bool IsValid { get; set; }

            public List<string> Errors { get; set; }
        }

        public static PasswordValidationResult ValidatePassword(string password)
        {
            var errors = new List<string>();

            if (password.Length < 8)
            {
                errors.Add("Password must be at least 8 characters long.");
            }
            if (!password.Any(char.IsDigit))
            {
                errors.Add("Password must contain at least one digit.");
            }
            if (!password.Any(char.IsLower))
            {
                errors.Add("Password must contain at least one lowercase letter.");
            }
            if (!password.Any(char.IsUpper))
            {
                errors.Add("Password must contain at least one uppercase letter.");
            }
            if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                errors.Add("Password must contain at least one non-alphanumeric character.");
            }

            return new PasswordValidationResult
            {
                IsValid = errors.Count == 0,
                Errors = errors
            };
        }


    }

   
}
