using FluentValidation;
using WebAPI.Queries.UserQueries;

namespace WebAPI.Validation.AbstractValidators
{
    public class RegisterAdminQueryValidator : AbstractValidator<RegisterAdminQuery>
    {
        public RegisterAdminQueryValidator()
        {
            // Validate FirstName
            RuleFor(x => x.Register.FirstName)
                .NotEmpty().WithMessage("First Name is required.")
                .MaximumLength(25).WithMessage("First Name cannot exceed 25 characters.");

            // Validate LastName
            RuleFor(x => x.Register.LastName)
                .NotEmpty().WithMessage("Last Name is required.")
                .MaximumLength(25).WithMessage("Last Name cannot exceed 25 characters.");

            // Validate Phone
            RuleFor(x => x.Register.Phone)
                .NotEmpty().WithMessage("Phone number is required.")
                .Must(IsValidPhoneNumber)
                .WithMessage("Please enter a valid phone number.");

            // Validate birthtime
            RuleFor(x => x.Register.birthtime)
                .Must(date => date != default(DateTime))
                .WithMessage("The Birth Date is not valid.")
                .Must(BeAValidDate)
                .WithMessage("The Birth Date must be a valid date.");

            // Validate Address
            RuleFor(x => x.Register.Address)
                .NotNull().WithMessage("Address is required.")
                .SetValidator(new RegisterAddressValidator()); 

            // Validate Role
            RuleFor(x => x.Register.Role)
                .NotEmpty().WithMessage("Role is required.")
                .Must(BeAValidRole)
                .WithMessage("Invalid role. Role must be one of the predefined roles.");

            // Validate Email
            RuleFor(x => x.Register.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email is required.");

            // Validate Password
            RuleFor(x => x.Register.EncryptedPassword)
                .NotEmpty().WithMessage("Password is required.");
        }

        // Custom Phone Validator
        private bool IsValidPhoneNumber(string phone)
        {
            var regex = new System.Text.RegularExpressions.Regex("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$");
            return regex.IsMatch(phone);
        }

        // Custom Birth Date Validator
        private bool BeAValidDate(DateTime date)
        {
            return date > DateTime.MinValue && date < DateTime.MaxValue;
        }

        // Custom Role Validator
        private bool BeAValidRole(string role)
        {
            // Assuming roles like "Admin", "User", etc. Modify as per your system's roles.
            var validRoles = new[] { "Admin", "Seller", "Buyer" };
            return validRoles.Contains(role);
        }
    }
}