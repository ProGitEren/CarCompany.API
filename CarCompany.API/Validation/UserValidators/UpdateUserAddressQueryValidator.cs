using FluentValidation;
using System.Security.Claims;
using WebAPI.Commands;
using WebAPI.Commands.UserCommands;

namespace WebAPI.Validation.UserValidators
{
    public class UpdateUserAddressCommandValidator : AbstractValidator<UpdateUserAddressCommand>
    {
        public UpdateUserAddressCommandValidator()
        {
            // Validate Address using AddressDtoValidator
            RuleFor(x => x.Address)
                .NotNull().WithMessage("Address is required.")
                .SetValidator(new AddressDtoValidator()); // Use AddressDtoValidator

            
        }
    }
}
