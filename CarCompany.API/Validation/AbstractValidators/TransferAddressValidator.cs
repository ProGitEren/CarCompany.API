using FluentValidation;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Validation.AbstractValidators
{
    public class TransferAddressValidator : AbstractValidator<TransferAddress>
    {
        public TransferAddressValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Address Name is required.")
                .MaximumLength(100).WithMessage("Address Name must not exceed 100 characters.");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required.")
                .MaximumLength(100).WithMessage("City must not exceed 100 characters.");

            RuleFor(x => x.State)
                .NotEmpty().WithMessage("State is required.")
                .MaximumLength(50).WithMessage("State must not exceed 50 characters.");

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Country is required.")
                .MaximumLength(50).WithMessage("Country must not exceed 50 characters.");

            RuleFor(x => x.ZipCode)
                .NotEmpty().WithMessage("Zip Code is required.")
                .Must(x => x.ToString().Length == 5).WithMessage("Zip Code must be 5 digits.");


        }
    }
}
