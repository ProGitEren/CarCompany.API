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
                   .UseIdentityColumn(seed: 1000, increment: 1);


            // Instances Configurations

            builder.Property(x => x.Quantity).IsRequired().HasDefaultValue(1);
            builder.Property(x => x.ManufacturedCountry).IsRequired();
            builder.Property(x => x.Manufacturer).IsRequired().HasColumnType("nchar(2)");
            builder.Property(x => x.ManufacturedPlant).IsRequired().HasColumnType("nchar(1)");
            builder.Property(x => x.ManufacturedYear).IsRequired().HasColumnType("nchar(1)");
            builder.Property(x => x.CheckDigit).IsRequired().HasColumnType("nchar(1)");
            builder.Property(x => x.EngineCode).IsRequired().HasColumnType("nchar(5)");
            builder.Property(x => x.ModelLongName).HasMaxLength(100);
            builder.Property(x => x.ModelShortName).HasMaxLength(20);
            builder.Property(x => x.VehicleType).HasConversion(o => o.ToString(), o => (VehicleType)Enum.Parse(typeof(VehicleType), o.ToString()) );

                    

            
            
            // Navigational Configurations

            //builder.HasMany(e => e.Vehicles)
            //    .WithOne(x => x.VehicleModel)
            //    .HasForeignKey(y=>y.ModelId)
            //    .OnDelete(DeleteBehavior.NoAction);



            // Seed Data

            // Seed Data
            builder.HasData(
                new VehicleModels
                {
                    Id = 1000,
                    CheckDigit = "1",
                    ModelYear = 2020,
                    ManufacturedCountry = 1,
                    ManufacturedPlant = "A",
                    ManufacturedYear = VinYearMapper.GetManufacturedYearCode(2020).ToString(),
                    EngineCode = "EN123",
                    Manufacturer = "TM",
                    ModelLongName = "Toyota Camry",
                    ModelShortName = "Camry",
                    VehicleType = VehicleType.Automobile
                },
                new VehicleModels
                {
                    Id = 1001,
                    CheckDigit = "2",
                    ModelYear = 2021,
                    ManufacturedCountry = 2,
                    ManufacturedPlant = "B",
                    ManufacturedYear = VinYearMapper.GetManufacturedYearCode(2021).ToString(),
                    EngineCode = "EN456",
                    Manufacturer = "FD",
                    ModelLongName = "Ford Mustang",
                    ModelShortName = "Mustang",
                    VehicleType = VehicleType.Automobile
                },
                new VehicleModels
                {
                    Id = 1002,
                    CheckDigit = "3",
                    ModelYear = 2019,
                    ManufacturedCountry = 1,
                    ManufacturedPlant = "C",
                    ManufacturedYear = VinYearMapper.GetManufacturedYearCode(2019).ToString(),
                    EngineCode = "EN789",
                    Manufacturer = "BM",
                    ModelLongName = "BMW X5",
                    ModelShortName = "X5",
                    VehicleType = VehicleType.SUV
                },
                new VehicleModels
                {
                    Id = 1003,
                    CheckDigit = "4",
                    ModelYear = 2018,
                    ManufacturedCountry = 1,
                    ManufacturedPlant = "D",
                    ManufacturedYear = VinYearMapper.GetManufacturedYearCode(2018).ToString(),
                    EngineCode = "EN101",
                    Manufacturer = "HN",
                    ModelLongName = "Honda Civic",
                    ModelShortName = "Civic",
                    VehicleType = VehicleType.Automobile
                },
                new VehicleModels
                {
                    Id = 1004,
                    CheckDigit = "5",
                    ModelYear = 2017,
                    ManufacturedCountry = 1,
                    ManufacturedPlant = "E",
                    ManufacturedYear = VinYearMapper.GetManufacturedYearCode(2017).ToString(),
                    EngineCode = "EN202",
                    Manufacturer = "NS",
                    ModelLongName = "Nissan Altima",
                    ModelShortName = "Altima",
                    VehicleType = VehicleType.Automobile
                }
            );


        }

    }
}
