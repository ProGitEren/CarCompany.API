using Microsoft.AspNetCore.Identity;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace ClassLibrary2.Entities
{
    public class AppUsers : IdentityUser
    {
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public DateTime birthtime { get; set; }

        //[NotMapped]
        //[AllowNull]
        public IList<string> roles { get; set; }

        //Navigational Properties

        public Guid? AddressId { get; set; }
        public virtual Address Address { get; set; }

        //public string? VehicleId { get; set; }
        public virtual ICollection<Vehicles> Vehicles { get; set; }


    }
}
