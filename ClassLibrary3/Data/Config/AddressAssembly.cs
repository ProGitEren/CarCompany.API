using ClassLibrary2.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Data.Config
{
    public class AddressAssembly : IEntityTypeConfiguration<Address>
    {

        public void Configure(EntityTypeBuilder<Address> builder)
        {
            //Key Configurations
            builder.HasKey(x => x.AddressId);
            builder.Property(x => x.AddressId)
            .IsRequired()
            .ValueGeneratedOnAdd();



            // Propery Configurations
            builder.Property(x => x.country).IsRequired();
            builder.Property(x => x.city).IsRequired();
            builder.Property(x => x.state).IsRequired();
            builder.Property(x => x.name)
            .IsRequired()
            .HasMaxLength(100)
            ;
            


        }

    }
}
