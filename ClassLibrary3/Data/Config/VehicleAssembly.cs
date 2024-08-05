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
    public class VehicleAssembly : IEntityTypeConfiguration<Vehicles>
    {
        public void Configure(EntityTypeBuilder<Vehicles> builder)
        {
            // Key Condiguration
            builder.HasKey(x => x.Vin);

            builder.Property(e => e.Vin)
                .IsRequired()
                .HasMaxLength(17)
                .IsFixedLength();

            // Instance Requirements 


            builder.Property(e => e.Averagefuelin).HasColumnType("decimal(18, 4)");
            builder.Property(e => e.Averagefuelout).HasColumnType("decimal(18, 4)");


            // One-To-Many Relationship
            builder.HasOne(e => e.VehicleModel)
                .WithMany(vm => vm.Vehicles)
                .HasForeignKey(e => e.ModelId);

            // One-To-Many Relationship
            builder.HasOne(e => e.Engine)
                .WithMany(en => en.Vehicles)
                .HasForeignKey(e => e.EngineId);

            // One-TO-One Relationship
            builder.HasOne(e => e.User)
                .WithOne(au => au.Vehicle)
                .HasForeignKey<Vehicles>(e => e.UserId);

            // Did not seed data as the properties will depend on the Model and Engine inside it therefore seeding will not be done through here as 
            // the objects are deeply relational it will be added cia the controller actions in API then followingly in UI only by ADMIN !!!!!



        }



    }
}
