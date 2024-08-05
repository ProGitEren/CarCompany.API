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
            builder
           .HasOne(u => u.Address)
           .WithOne() // Assuming Address does not have a navigation property to AppUsers
           .HasForeignKey<AppUsers>(u => u.AddressId)
           .OnDelete(DeleteBehavior.SetNull); // Disable cascade delete

          

        }
    }
}
