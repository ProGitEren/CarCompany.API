using ClassLibrary2.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Data.Config
{
    public class UsersAssembly : IEntityTypeConfiguration<AppUsers>
    {
        public void Configure(EntityTypeBuilder<AppUsers> builder)
        {

            // Key Configuration 
            // Not needed as Microsoft.Identity already handles it on it's own

            // Instances Configuration
            builder.Property(x => x.FirstName).IsRequired().HasMaxLength(25);
            builder.Property(x => x.LastName).IsRequired().HasMaxLength(25);           
            builder.Property(x => x.birthtime).IsRequired();
            builder.Property(x => x.Phone).IsRequired();
            
            builder.Ignore(x => x.roles);
            builder.Property(x => x.roles).IsRequired(false);


            // Relational Configurations

            builder
           .HasOne(u => u.Address)
           .WithOne() // Assuming Address does not have a navigation property to AppUsers
           .HasForeignKey<AppUsers>(u => u.AddressId)
           .OnDelete(DeleteBehavior.SetNull); // Disable cascade delete

           builder
          .HasOne(u => u.Vehicle)
          .WithOne() // Assuming Address does not have a navigation property to AppUsers
          .HasForeignKey<AppUsers>(u => u.VehicleId)
          .OnDelete(DeleteBehavior.SetNull); // Disable cascade delete



        }
    }
}
