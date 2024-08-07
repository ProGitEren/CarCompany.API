using Infrastucture.Data.ValueGenerators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities;
using Models.Enums;
using Models.Mapping;
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
                   .HasValueGenerator<CustomIdValueGenerator<VehicleModels>>() // Use custom value generator
                   .IsRequired();


            // Instances Configurations

            builder.Property(x => x.Quantity).IsRequired().HasDefaultValue(1);
            builder.Property(x => x.securityCode).IsRequired().HasColumnType("nchar(1)");
            builder.Property(x => x.ManufacturedCountry).IsRequired();
            builder.Property(x => x.securityCode).IsRequired().HasColumnType("nchar(1)");
            builder.Property(x => x.Manufacturer).IsRequired().HasColumnType("nchar(2)");
            builder.Property(x => x.ManufacturedPlant).IsRequired().HasColumnType("nchar(1)");
            builder.Property(x => x.ManufacturedYear).IsRequired().HasColumnType("nchar(1)");
            builder.Property(x => x.CheckDigit).IsRequired().HasColumnType("nchar(1)");
            builder.Property(x => x.EngineCode).IsRequired().HasColumnType("nchar(6)");
            builder.Property(x => x.ModelLongName).HasMaxLength(100);
            builder.Property(x => x.ModelShortName).HasMaxLength(20);

            builder.Property(x => x.VehicleType).HasConversion(o => o.ToString(), o => (VehicleType)Enum.Parse(typeof(VehicleType), o.ToString()) );


            // Navigational Configurations

            builder.HasMany(e => e.Vehicles)
                .WithOne(x => x.VehicleModel)
                .HasForeignKey(y=>y.ModelId)
                .OnDelete(DeleteBehavior.NoAction);



            // Seed Data

            builder.HasData(
                new VehicleModels

                {
                    Id = 1000,
                    CheckDigit = "1",
                    ModelYear = 2000,
                    ManufacturedCountry = 1,
                    ManufacturedPlant = "A",
                    ManufacturedYear = VinYearMapper.GetManufacturedYearCode(2000).ToString(),
                    EngineCode = "AB1234",
                    securityCode = "A",
                    Manufacturer = "A",
                    ModelLongName = "TestLongName",
                    ModelShortName = "TestShortName",
                    VehicleType = VehicleType.Automobile


                });

           
        }

    }
}
