﻿using Infrastucture.Data.ValueGenerators;
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

            builder.Property(x => x.EngineCode).IsRequired().HasColumnType("nchar(6)");


            // Navigational Configurations

            builder.HasMany(e => e.Vehicles)
                .WithOne(x => x.Engine)
                .HasForeignKey(y => y.EngineId)
                .OnDelete(DeleteBehavior.NoAction);


            // Seed Data

            builder.HasData(
                new Engines 
                {
                    Id = 1000,
                    Volume = 1000,
                    Hp = 200,
                    Torque = 250,
                    CompressionRatio = 5,
                    diameterCm = 55,
                    EngineCode = "AB1234"

                }
                
                
                
                );

        }
    }
}
