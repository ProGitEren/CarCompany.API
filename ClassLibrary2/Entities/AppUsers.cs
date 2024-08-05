using Microsoft.AspNetCore.Identity;
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


        public Guid? AddressId { get; set; }
 
        //[Required]
        public virtual Address Address { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime birthtime { get; set; }

        [NotMapped]
        [AllowNull]
        public IList<string> roles { get; set; }


    }
}
