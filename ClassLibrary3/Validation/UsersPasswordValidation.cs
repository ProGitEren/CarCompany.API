//using ClassLibrary2.Entities;
//using Microsoft.AspNetCore.Identity;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Infrastucture.Validation
//{
//    public class UsersPasswordValidation : IPasswordValidator<AppUsers>
//    {
//        public Task<IdentityResult> ValidateAsync(UserManager<AppUsers> manager, AppUsers user, string password)
//        {
//            List<IdentityError> errors = new List<IdentityError>();
//            if (password.Length < 8) //Password karakter sayısı
//                errors.Add(new IdentityError { Code = "PasswordLength", Description = "Password should be at least 8 characters long." });
//            if (!password.Any(char.IsDigit))
//                errors.Add(new IdentityError { Code = "PasswordDigit", Description = "Password should at least contain 1 digit." });
//            if (!password.Any(char.IsLower))
//                errors.Add(new IdentityError { Code = "PasswordLowerCase", Description = "Password should at least contain 1 lower case." });
//            if (!password.Any(char.IsUpper))
//                errors.Add(new IdentityError { Code = "PasswordUpperCase", Description = "Password should at least contain 1 upper case." });
//            if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
//                errors.Add(new IdentityError { Code = "PasswordUniqueChar", Description = "Password should at least contain 1 special case." });
//            if (!errors.Any())
//                return Task.FromResult(IdentityResult.Success);
//            else
//                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
//        }
//    }
//}
