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
                   .UseIdentityColumn(seed: 1000, increment: 1);


            // Instances Configurations

            builder.Property(x => x.Volume).IsRequired().HasColumnType("decimal(18,4)");
            builder.Property(x => x.diameterCm).IsRequired().HasColumnType("decimal(18,4)");
            builder.Property(x => x.CompressionRatio).IsRequired();
            builder.Property(x => x.Torque).IsRequired();
            builder.Property(x => x.Hp).IsRequired();

            builder.Property(x => x.EngineCode).IsRequired().HasColumnType("nchar(5)");


            // Navigational Configurations

            //builder.HasMany(e => e.Vehicles)
            //    .WithOne(x => x.Engine)
            //    .HasForeignKey(y => y.EngineId)
            //    .OnDelete(DeleteBehavior.NoAction);


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
                },
                new Engines
                {
                    Id = 1005,
                    Volume = 5.0m,
                    Hp = 450,
                    Torque = 500,
                    CompressionRatio = 12,
                    diameterCm = 12.0m,
                    EngineCode = "F1V8"
                },
                new Engines
                {
                    Id = 1006,
                    Volume = 0.0m, // Electric motors might not have a traditional volume
                    Hp = 200,
                    Torque = 300,
                    CompressionRatio = 0, // Not applicable for electric motors
                    diameterCm = 0.0m, // Not applicable for electric motors
                    EngineCode = "F2EM"
                },
                new Engines
                {
                    Id = 1007,
                    Volume = 2.5m,
                    Hp = 350,
                    Torque = 450,
                    CompressionRatio = 11,
                    diameterCm = 9.5m,
                    EngineCode = "F3HY"
                },
                new Engines
                {
                    Id = 1008,
                    Volume = 3.0m,
                    Hp = 350,
                    Torque = 400,
                    CompressionRatio = 10,
                    diameterCm = 10.5m,
                    EngineCode = "F4V6"
                },
                new Engines
                {
                    Id = 1009,
                    Volume = 4.5m,
                    Hp = 400,
                    Torque = 450,
                    CompressionRatio = 11,
                    diameterCm = 11.5m,
                    EngineCode = "T1V8"
                },
                new Engines
                {
                    Id = 1010,
                    Volume = 0.0m,
                    Hp = 250,
                    Torque = 350,
                    CompressionRatio = 0,
                    diameterCm = 0.0m,
                    EngineCode = "T2EM"
                },
                new Engines
                {
                    Id = 1011,
                    Volume = 2.5m,
                    Hp = 300,
                    Torque = 400,
                    CompressionRatio = 10,
                    diameterCm = 9.5m,
                    EngineCode = "T3HY"
                },
                new Engines
                {
                    Id = 1012,
                    Volume = 3.0m,
                    Hp = 325,
                    Torque = 375,
                    CompressionRatio = 10,
                    diameterCm = 10.5m,
                    EngineCode = "T4V6"
                },
                new Engines
                {
                    Id = 1013,
                    Volume = 4.5m,
                    Hp = 450,
                    Torque = 500,
                    CompressionRatio = 12,
                    diameterCm = 12.0m,
                    EngineCode = "B1V8"
                },
                new Engines
                {
                    Id = 1014,
                    Volume = 0.0m,
                    Hp = 250,
                    Torque = 350,
                    CompressionRatio = 0,
                    diameterCm = 0.0m,
                    EngineCode = "B2EM"
                },
                new Engines
                {
                    Id = 1015,
                    Volume = 2.5m,
                    Hp = 300,
                    Torque = 400,
                    CompressionRatio = 11,
                    diameterCm = 9.5m,
                    EngineCode = "B3HY"
                },
                new Engines
                {
                    Id = 1016,
                    Volume = 3.0m,
                    Hp = 325,
                    Torque = 375,
                    CompressionRatio = 10,
                    diameterCm = 10.5m,
                    EngineCode = "B4V6"
                },
                new Engines
                {
                    Id = 1017,
                    Volume = 4.5m,
                    Hp = 450,
                    Torque = 500,
                    CompressionRatio = 12,
                    diameterCm = 12.0m,
                    EngineCode = "H1V8"
                },
                new Engines
                {
                    Id = 1018,
                    Volume = 0.0m,
                    Hp = 250,
                    Torque = 350,
                    CompressionRatio = 0,
                    diameterCm = 0.0m,
                    EngineCode = "H2EM"
                },
                new Engines
                {
                    Id = 1019,
                    Volume = 2.5m,
                    Hp = 300,
                    Torque = 400,
                    CompressionRatio = 11,
                    diameterCm = 9.5m,
                    EngineCode = "H3HY"
                },
                new Engines
                {
                    Id = 1020,
                    Volume = 3.0m,
                    Hp = 325,
                    Torque = 375,
                    CompressionRatio = 10,
                    diameterCm = 10.5m,
                    EngineCode = "H4V6"
                },
                new Engines
                {
                    Id = 1021,
                    Volume = 4.5m,
                    Hp = 450,
                    Torque = 500,
                    CompressionRatio = 12,
                    diameterCm = 12.0m,
                    EngineCode = "M1V8"
                },
                new Engines
                {
                    Id = 1022,
                    Volume = 0.0m,
                    Hp = 250,
                    Torque = 350,
                    CompressionRatio = 0,
                    diameterCm = 0.0m,
                    EngineCode = "M2EM"
                },
                new Engines
                {
                    Id = 1023,
                    Volume = 2.5m,
                    Hp = 300,
                    Torque = 400,
                    CompressionRatio = 11,
                    diameterCm = 9.5m,
                    EngineCode = "M3HY"
                },
                new Engines
                {
                    Id = 1024,
                    Volume = 3.0m,
                    Hp = 325,
                    Torque = 375,
                    CompressionRatio = 10,
                    diameterCm = 10.5m,
                    EngineCode = "M4V6"
                }
            );

        }
    }
}
