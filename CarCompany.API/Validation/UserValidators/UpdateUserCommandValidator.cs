using FluentValidation;
using WebAPI.Commands.UserCommands;
using Infrastucture.DTO.Dto_Users;
using System.Security.Claims;

namespace WebAPI.Validation.AbstractValidators
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            // Validate FirstName
            RuleFor(x => x.UpdateUserDto.FirstName)
                .NotEmpty().WithMessage("First Name is required.")
                .MaximumLength(25).WithMessage("First Name cannot exceed 25 characters.");

            // Validate LastName
            RuleFor(x => x.UpdateUserDto.LastName)
                .NotEmpty().WithMessage("Last Name is required.")
                .MaximumLength(25).WithMessage("Last Name cannot exceed 25 characters.");

            // Validate Email
            RuleFor(x => x.UpdateUserDto.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.");


            // Optional: Validate specific claims if needed
            RuleFor(x => x.Claim)
                .Must(claim =>  claim.Type == ClaimTypes.Email)
                .WithMessage("User must have a valid email claim.");
        }
    }
}