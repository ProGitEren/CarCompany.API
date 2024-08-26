using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Models.Enums.OrderEnums;

namespace Infrastucture.Data.Config
{
    public class OrderAssembly : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder) 
        {
            // Key Condiguration

            builder.HasKey(x => x.Id);

            builder.Property(e => e.Id)
                .IsRequired()
                .UseIdentityColumn(seed:1000, increment: 1);


            // Instance Requirements 


            builder.Property(e => e.OrderedDate).IsRequired();
            builder.Property(e => e.SellerEmail).IsRequired();
            builder.Property(e => e.BuyerEmail).IsRequired();
            builder.Property(e => e.OrderType).IsRequired().HasConversion(x => x.ToString(), x => (OrderType)Enum.Parse(typeof(OrderType),x.ToString()));
            builder.Property(e => e.OrderStatus).IsRequired().HasConversion(x => x.ToString(), x => (OrderStatus)Enum.Parse(typeof(OrderStatus),x.ToString()));
            builder.Property(e => e.PaymentMethod).IsRequired().HasConversion(x => x.ToString(), x => (PaymentMethod)Enum.Parse(typeof(PaymentMethod),x.ToString()));

            builder.Property(x => x.OrderVehicleId).IsRequired();

            // Owning Properties

            builder.OwnsOne(x => x.TransferAddress, x => x.WithOwner());
            
            builder
                .HasOne(x => x.Vehicle)
                .WithOne(y => y.Order)
                .HasForeignKey<Order>(x => x.OrderVehicleId)
                .OnDelete(DeleteBehavior.Cascade);
            

        }
    }
}
