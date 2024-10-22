using FluentValidation;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Validation.AbstractValidators
{
    public class VehicleModelValidator : AbstractValidator<VehicleModels>
    {
        public VehicleModelValidator()
        {
            // Ensuring all fields are not empty or null
            RuleFor(x => x.VehicleType)
                .NotEmpty()
                .IsInEnum()
                .WithMessage("Invalid Vehicle Type.");

            RuleFor(x => x.EngineCode)
                .NotEmpty()
                .Length(5)
                .WithMessage("Engine Code should be exactly 5 digits (letters/numbers).");

            RuleFor(x => x.ModelShortName)
                .NotEmpty()
                .MaximumLength(20)
                .WithMessage("Model Short Name cannot exceed 20 characters.");

            RuleFor(x => x.ModelLongName)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("Model Long Name cannot exceed 100 characters.");

            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Quantity cannot be less than 0.");

            RuleFor(x => x.ModelYear)
                .NotEmpty()
                .InclusiveBetween(1980, DateTime.Now.Year)
                .WithMessage($"Model Year must be between 1980 and {DateTime.Now.Year}.");

            RuleFor(x => x.ModelPicture)
                .NotEmpty()
                .WithMessage("Model Picture is required.");

            RuleFor(x => x.Price)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("Price must be greater than 0.");

            RuleFor(x => x.ManufacturedCountry)
                .NotEmpty()
                .Must(value => new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.Contains(value))
                .WithMessage("Manufactured Country must be one of the allowed values.");

            RuleFor(x => x.Manufacturer)
                .NotEmpty()
                .Length(2)
                .WithMessage("Manufacturer must be exactly 2 letters.");

            RuleFor(x => x.ManufacturedYear)
                .NotEmpty()
                .Must(BeValidManufacturedYear)
                .WithMessage("Invalid Manufactured Year.");

            RuleFor(x => x.ManufacturedPlant)
                .NotEmpty()
                .Must(BeValidManufacturedPlant)
                .WithMessage("Invalid Manufactured Plant.");

            RuleFor(x => x.CheckDigit)
                .NotEmpty()
                .Must(BeValidCheckDigit)
                .WithMessage("Invalid Check Digit.");
        }

        private bool BeValidManufacturedYear(string manufacturedYear)
        {
            return "ABCDEFGHJKLMNPRSTVWXY1234567890".Contains(manufacturedYear);
        }

        private bool BeValidManufacturedPlant(string manufacturedPlant)
        {
            return "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".Contains(manufacturedPlant);
        }

        private bool BeValidCheckDigit(string checkDigit)
        {
            return "0123456789X".Contains(checkDigit);
        }
    }

}

