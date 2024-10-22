using FluentValidation;
using Models.Entities;

namespace WebAPI.Validation
{
    public class OrderVehicleValidator : AbstractValidator<OrderVehicle>
    {
        public OrderVehicleValidator()
        {
            RuleFor(x => x.VehicleId)
                .NotEmpty().WithMessage("Vehicle ID is required.")
                .Length(17).WithMessage("Vehicle ID must be exactly 17 characters."); // Assuming Vehicle ID is like a VIN

            RuleFor(x => x.ModelName)
                .NotEmpty().WithMessage("Model Name is required.")
                .MaximumLength(25).WithMessage("Model Name cannot exceed 25 characters.");

            RuleFor(x => x.PictureFolderPath)
                .NotEmpty().WithMessage("Picture Folder Path is required.")
                .Must(IsValidPath).WithMessage("Picture Folder Path is not a valid path.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.")
                .ScalePrecision(2, 18).WithMessage("Price cannot have more than 18 digits in total, with 2 decimal places.");
        }

        private bool IsValidPath(string path)
        {
            return Uri.IsWellFormedUriString(path, UriKind.RelativeOrAbsolute);
        }
    }
}
