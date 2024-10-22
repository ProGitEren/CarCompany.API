using FluentValidation;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Validation.AbstractValidators
{
    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            // BuyerEmail (Optional)
            RuleFor(x => x.BuyerEmail)
                .EmailAddress()
                .WithMessage("Not a valid Email Address.");

            // SellerEmail
            RuleFor(x => x.SellerEmail)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Not a valid Email Address.");

            // OrderedDate
            RuleFor(x => x.OrderedDate)
                .NotEmpty()
                .WithMessage("Ordered Date is required.");

            // OrderStatus
            RuleFor(x => x.OrderStatus)
                .NotNull()
                .WithMessage("Order Status is required.");

            // OrderType
            RuleFor(x => x.OrderType)
                .NotNull()
                .WithMessage("Order Type is required.");

            // PaymentMethod
            RuleFor(x => x.PaymentMethod)
                .NotNull()
                .WithMessage("Payment Method is required.");

            // TransferAddress
            RuleFor(x => x.TransferAddress)
                .NotNull()
                .SetValidator(new TransferAddressValidator())
                .WithMessage("Transfer Address is required.");

            // IsVehicleOwnerTransferred
            RuleFor(x => x.IsVehicleOwnerTransferred)
                .NotNull()
                .WithMessage("Is Vehicle Owner Transferred must be set.");

            // OrderVehicleId
            RuleFor(x => x.OrderVehicleId)
                .NotEmpty()
                .WithMessage("Order Vehicle ID is required.");

           

        }

    }
}
