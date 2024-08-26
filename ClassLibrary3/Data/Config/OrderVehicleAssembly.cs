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
    public class OrderVehicleAssembly : IEntityTypeConfiguration<OrderVehicle> 
    {
        public void Configure(EntityTypeBuilder<OrderVehicle> builder) 
        {
            builder.HasKey(x => x.Id);

            builder.Property(u => u.Id)
                   .UseIdentityColumn(seed: 1000, increment: 1);

            builder.Property(x => x.Price).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.ModelName).IsRequired().HasMaxLength(25);
            builder.Property(x => x.PictureFolderPath).IsRequired();


        }
    }
}
