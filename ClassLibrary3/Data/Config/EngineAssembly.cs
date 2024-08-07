using Infrastucture.Data.ValueGenerators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Data.Config
{
    public class EngineAssembly : IEntityTypeConfiguration<Engines>
    {

        public void Configure(EntityTypeBuilder<Engines> builder) 
        {
            //Key Configurations

            builder.HasKey(x => x.Id);

            builder.Property(u => u.Id)
                   .HasValueGenerator<CustomIdValueGenerator<Engines>>() // Use custom value generator
                   .IsRequired();


            // Instances Configurations

            builder.Property(x => x.Volume).IsRequired().HasColumnType("decimal(18,4)");
            builder.Property(x => x.diameterCm).IsRequired().HasColumnType("decimal(18,4)");
            builder.Property(x => x.CompressionRatio).IsRequired();
            builder.Property(x => x.Torque).IsRequired();
            builder.Property(x => x.Hp).IsRequired();

            builder.Property(x => x.EngineCode).IsRequired().HasColumnType("nchar(5)");


            // Navigational Configurations

            builder.HasMany(e => e.Vehicles)
                .WithOne(x => x.Engine)
                .HasForeignKey(y => y.EngineId)
                .OnDelete(DeleteBehavior.NoAction);


            // Seed Data

            // Seed Data
            builder.HasData(
                new Engines
                {
                    Id = 1000,
                    Volume = 2.0m,
                    Hp = 150,
                    Torque = 200,
                    CompressionRatio = 10,
                    diameterCm = 8.5m,
                    EngineCode = "EN123"
                },
                new Engines
                {
                    Id = 1001,
                    Volume = 3.5m,
                    Hp = 250,
                    Torque = 350,
                    CompressionRatio = 11,
                    diameterCm = 10.0m,
                    EngineCode = "EN456"
                },
                new Engines
                {
                    Id = 1002,
                    Volume = 4.0m,
                    Hp = 300,
                    Torque = 400,
                    CompressionRatio = 12,
                    diameterCm = 11.0m,
                    EngineCode = "EN789"
                },
                new Engines
                {
                    Id = 1003,
                    Volume = 1.6m,
                    Hp = 120,
                    Torque = 180,
                    CompressionRatio = 9,
                    diameterCm = 7.5m,
                    EngineCode = "EN101"
                },
                new Engines
                {
                    Id = 1004,
                    Volume = 2.5m,
                    Hp = 200,
                    Torque = 250,
                    CompressionRatio = 10,
                    diameterCm = 9.0m,
                    EngineCode = "EN202"
                }
            );

        }
    }
}
