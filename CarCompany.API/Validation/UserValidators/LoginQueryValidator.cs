using FluentValidation;
using WebAPI.Queries.UserQueries;

namespace WebAPI.Validation.UserValidators
{
    public class LoginQueryValidator : AbstractValidator<LoginQuery>
    {
        public LoginQueryValidator()
        {
            RuleFor(x => x.Login.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Not a Valid Email Address.");
            
            RuleFor(x => x.Login.EncryptedPassword)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}
