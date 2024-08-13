using Infrastucture.Data.ValueGenerators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities;
using Models.Entities.List_of_Entities;
using Models.Enums;
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
            builder.Property(x => x.EngineName).IsRequired();
            builder.Property(x => x.Cylinder).HasConversion(o => o.ToString(), o => (Cylinder)Enum.Parse(typeof(Cylinder), o.ToString()));


            builder.Property(x => x.EngineCode).IsRequired().HasColumnType("nchar(5)");


            // Navigational Configurations

            //builder.HasMany(e => e.Vehicles)
            //    .WithOne(x => x.Engine)
            //    .HasForeignKey(y => y.EngineId)
            //    .OnDelete(DeleteBehavior.NoAction);


            // Seed Data

            // Seed Data

            builder.HasData(EntityList.EnginesList.ToArray());
            

        }
    }
}
