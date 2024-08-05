using ClassLibrary2.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace Infrastucture.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUsers>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
        
        }
        
        public DbSet<Address> addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUsers>()
            .HasOne(u => u.Address)
            .WithOne() // Assuming Address does not have a navigation property to AppUsers
            .HasForeignKey<AppUsers>(u => u.AddressId)
            .OnDelete(DeleteBehavior.SetNull); // Disable cascade delete

            modelBuilder.Entity<Address>()
           .HasKey(u => u.AddressId);


        }
    }

    
}
