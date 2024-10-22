using ClassLibrary2.Entities;
using FluentValidation;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Validation.AbstractValidators
{
    public class AddressValidator : AbstractValidator<Address>
    {
        public AddressValidator()
        {
            RuleFor(x => x.name)
                .NotEmpty().WithMessage("Address Name is required.")
                .MaximumLength(100).WithMessage("Address Name must not exceed 100 characters.");

            RuleFor(x => x.city)
                .NotEmpty().WithMessage("City is required.")
                .MaximumLength(100).WithMessage("City must not exceed 100 characters.");

            RuleFor(x => x.state)
                .NotEmpty().WithMessage("State is required.")
                .MaximumLength(50).WithMessage("State must not exceed 50 characters.");

            RuleFor(x => x.country)
                .NotEmpty().WithMessage("Country is required.")
                .MaximumLength(50).WithMessage("Country must not exceed 50 characters.");

            RuleFor(x => x.zipcode)
                .NotEmpty().WithMessage("Zip Code is required.")
                .Must(x => x.ToString().Length == 5).WithMessage("Zip Code must be 5 digits.");


        }
    }
}