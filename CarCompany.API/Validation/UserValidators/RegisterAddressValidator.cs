using FluentValidation;
using Infrastucture.DTO.Dto_Address;

namespace WebAPI.Validation.AbstractValidators
{
    public class RegisterAddressValidator : AbstractValidator<RegisterAddressDto>
    {
        public RegisterAddressValidator()
        {
            // Validate Name
            RuleFor(x => x.name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            // Validate City
            RuleFor(x => x.city)
                .NotEmpty().WithMessage("City is required.")
                .MaximumLength(50).WithMessage("City cannot exceed 50 characters.");

            // Validate State
            RuleFor(x => x.state)
                .NotEmpty().WithMessage("State is required.")
                .MaximumLength(50).WithMessage("State cannot exceed 50 characters.");

            // Validate Zipcode
            RuleFor(x => x.zipcode)
                .InclusiveBetween(10000, 99999).WithMessage("Zipcode must be a 5-digit number.");

            // Validate Country
            RuleFor(x => x.country)
                .NotEmpty().WithMessage("Country is required.")
                .MaximumLength(50).WithMessage("Country cannot exceed 50 characters.");
        }
    }
}