using ClassLibrary2.Entities;
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
                .HasColumnType("nchar(17)");


            // Instance Requirements 


            builder.Property(e => e.Averagefuelin).HasColumnType("decimal(18, 4)").IsRequired();
            builder.Property(e => e.Averagefuelout).HasColumnType("decimal(18, 4)").IsRequired();
            builder.Property(e => e.DrivenKM).IsRequired();
            builder.Property(e => e.BaggageVolume).IsRequired();
            builder.Property(e => e.COemmission).IsRequired();
            builder.Property(e => e.MinWeight).IsRequired();
            builder.Property(e => e.MaxAllowedWeight).IsRequired();
            builder.Property(e => e.FuelCapacity).IsRequired();
            builder.Property(e => e.DrivenKM).IsRequired();
            builder.Property(e => e.UserName).IsRequired().HasMaxLength(25);
                    
            
            // Vehicle should have a model and engine
            builder.Property(x => x.ModelId).IsRequired();
            builder.Property(x => x.EngineId).IsRequired();
            builder.Property(x => x.UserId).IsRequired(false);



            //Navigational Configurations

            // One-To-Many Relationship
            builder.HasOne(e => e.VehicleModel)
                .WithMany(vm => vm.Vehicles)
                .HasForeignKey(e => e.ModelId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-To-Many Relationship
            builder.HasOne(e => e.Engine)
                .WithMany(en => en.Vehicles)
                .HasForeignKey(e => e.EngineId)
                .OnDelete(DeleteBehavior.Cascade);

            //One-To-Many Relationship
            builder.HasOne(e => e.User)
                .WithMany(x => x.Vehicles)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.SetNull); // Set Null so that anothe ruser will be related to this vehicle after the vehicle is sold


            // Did not seed data as the properties will depend on the Model and Engine inside it therefore seeding will not be done through here as 
            // the objects are deeply relational it will be added cia the controller actions in API then followingly in UI only by ADMIN !!!!!



        }



    }
}
