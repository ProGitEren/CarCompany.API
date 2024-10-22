using FluentValidation;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Validation.AbstractValidators
{
    public class VehicleValidator : AbstractValidator<Vehicles>
    {
        public VehicleValidator()
        {
            // VIN
            RuleFor(x => x.Vin)
                .NotEmpty()
                .Length(17)
                .WithMessage("VIN must be exactly 17 characters long.");

            // ModelName
            RuleFor(x => x.ModelName)
                .NotEmpty()
                .WithMessage("Model Name is required.");

            // EngineName
            RuleFor(x => x.EngineName)
                .NotEmpty()
                .WithMessage("Engine Name is required.");

            // UserName
            RuleFor(x => x.UserName)
                .NotEmpty()
                .MaximumLength(25)
                .WithMessage("User Name is required and should not exceed 25 characters.");

            // AverageFuelIn
            RuleFor(x => x.Averagefuelin)
                .NotEmpty()
                .GreaterThanOrEqualTo(0)
                .WithMessage("Average Fuel In must be a positive value.");

            // AverageFuelOut
            RuleFor(x => x.Averagefuelout)
                .NotEmpty()
                .GreaterThanOrEqualTo(0)
                .WithMessage("Average Fuel Out must be a positive value.");

            // COemmission
            RuleFor(x => x.COemmission)
                .NotEmpty()
                .InclusiveBetween(0, 250)
                .WithMessage("CO2 emission should be between 0 and 250.");

            // FuelCapacity
            RuleFor(x => x.FuelCapacity)
                .NotEmpty()
                .InclusiveBetween(0, 200)
                .WithMessage("Fuel Capacity should be between 0 and 200.");

            // MaxAllowedWeight
            RuleFor(x => x.MaxAllowedWeight)
                .NotEmpty()
                .InclusiveBetween(0, 30000)
                .WithMessage("Max Allowed Weight should be between 0 and 30000.");

            // MinWeight
            RuleFor(x => x.MinWeight)
                .NotEmpty()
                .InclusiveBetween(0, 10000)
                .WithMessage("Min Weight should be between 0 and 10000.");

            // BaggageVolume
            RuleFor(x => x.BaggageVolume)
                .NotEmpty()
                .InclusiveBetween(0, 1000)
                .WithMessage("Baggage Volume should be between 0 and 1000.");

            // DrivenKM
            RuleFor(x => x.DrivenKM)
                .NotEmpty()
                .GreaterThanOrEqualTo(0)
                .WithMessage("Driven KM must be a positive value.");

            // ModelId
            RuleFor(x => x.ModelId)
                .NotEmpty()
                .WithMessage("Model ID is required.");

            // EngineId
            RuleFor(x => x.EngineId)
                .NotEmpty()
                .WithMessage("Engine ID is required.");

            // ModelCode (Optional)
            RuleFor(x => x.ModelCode)
                .MaximumLength(5)
                .WithMessage("Model Code can be a maximum of 5 characters.");

            // UserId can be null
            
        }
    }
}
