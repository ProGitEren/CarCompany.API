using FluentValidation;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Validation.AbstractValidators
{
    public class EngineValidator : AbstractValidator<Engines>
    {
        public EngineValidator()
        {
            RuleFor(x => x.Cylinder)
                .NotEmpty()
                .WithMessage("Cylinder is required.");

            RuleFor(x => x.EngineName)
                .NotEmpty()
                .WithMessage("Engine Name is required.");

            RuleFor(x => x.Volume)
                .NotEmpty()
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(20)
                .WithMessage("The volume of the Engine should not exceed 20 L.");

            RuleFor(x => x.Hp)
                .NotEmpty()
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(3000)
                .WithMessage("The Horse Power of the Engine should not exceed 3000 Hp.");

            RuleFor(x => x.CompressionRatio)
                .NotEmpty()
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(25)
                .WithMessage("The Compression Ratio of the Engine should not exceed 25:1.");

            RuleFor(x => x.Torque)
                .NotEmpty()
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(4000)
                .WithMessage("The Torque of the Engine should not exceed 4000 N.m.");

            RuleFor(x => x.diameterCm)
                .NotEmpty()
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(200)
                .WithMessage("The Diameter (Bore) of the Engine should not exceed 200 mm.");

            RuleFor(x => x.EngineCode)
                .NotEmpty()
                .Length(5)
                .WithMessage("Engine Code should be exactly 5 digits (letter/number).");
        }
    }
}
