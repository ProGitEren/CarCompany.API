using ClassLibrary2.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Validation.AbstractValidators
{
    public class AppUserValidator : AbstractValidator<AppUsers>
    {
        public AppUserValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name is required.")
                .MaximumLength(25).WithMessage("First Name cannot exceed 25 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name is required.")
                .MaximumLength(25).WithMessage("Last Name cannot exceed 25 characters.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone number is required.")
                .Must(IsValidPhoneNumber)
                .WithMessage("Please enter a valid phone number.");

            RuleFor(x => x.birthtime)
                .Must(date => date != default(DateTime))
                .WithMessage("The Birth Date is not valid.")
                .Must(BeAValidDate)
                .WithMessage("The Birth Date must be a valid date.");

            RuleFor(x => x.AddressId)
                .NotEmpty().WithMessage("Address is required when available.");

            RuleFor(x => x.Vehicles)
                .NotEmpty().WithMessage("User must have at least one vehicle.")
                .When(x => x.Vehicles != null && x.Vehicles.Any());
        }

        private bool IsValidPhoneNumber(string phone)
        {
            var regex = new System.Text.RegularExpressions.Regex("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$");
            return regex.IsMatch(phone);
        }

        private bool BeAValidDate(DateTime date)
        {
            return date > DateTime.MinValue && date < DateTime.MaxValue;
        }
    }
}
