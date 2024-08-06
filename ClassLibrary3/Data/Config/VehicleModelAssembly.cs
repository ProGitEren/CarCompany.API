using Infrastucture.Data.ValueGenerators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Data.Config
{
    public class VehicleModelAssembly : IEntityTypeConfiguration<VehicleModels> 
    {
        public void Configure(EntityTypeBuilder<VehicleModels> builder) 
        {   
            // Key Configurations 

            builder.HasKey(x => x.Id);

            builder.Property(u => u.Id)
                   .HasValueGenerator<CustomIdValueGenerator>() // Use custom value generator
                   .IsRequired();


            // Instances Configurations

            builder.Property(x => x.Quantity).IsRequired().HasDefaultValue(1);
            builder.Property(x => x.securityCode).IsRequired().HasMaxLength(1).IsFixedLength();
            builder.Property(x => x.ManufacturedCountry).HasMaxLength(1).IsRequired();
            builder.Property(x => x.securityCode).HasMaxLength(1).IsFixedLength();
            builder.Property(x => x.Manufacturer).HasMaxLength(2).IsFixedLength();
            builder.Property(x => x.ManufacturedPlant).HasMaxLength(1).IsFixedLength();
            builder.Property(x => x.ManufacturedYear).HasMaxLength(1).IsFixedLength();
            builder.Property(x => x.CheckDigit).HasMaxLength(1).IsFixedLength();
            builder.Property(x => x.ModelCode).HasMaxLength(6).IsFixedLength();
            builder.Property(x => x.ModelLongName).HasMaxLength(100);
            builder.Property(x => x.ModelShortName).HasMaxLength(20);

            builder.Property(x => x.VehicleType).HasConversion(o => o.ToString(), o => (VehicleType)Enum.Parse(typeof(VehicleType), o.ToString()) );


            // Navigational Configurations

            builder.HasMany(e => e.Vehicles)
                .WithOne(x => x.VehicleModel)
                .HasForeignKey(y=>y.ModelId)
                .OnDelete(DeleteBehavior.NoAction);

           
        }

    }
}
