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
        [Required]
        [MaxLength(25, ErrorMessage = " Maximum 25 characters")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(25,ErrorMessage = " Maximum 25 characters")]
        public string LastName { get; set; }


       

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime birthtime { get; set; }

        [NotMapped]
        [AllowNull]
        public IList<string> roles { get; set; }

        //Navigational Properties

        public Guid? AddressId { get; set; }

        public virtual Address Address { get; set; }

        public string VehicleId { get; set; }

        public virtual Vehicles Vehicle { get; set; }


    }
}
