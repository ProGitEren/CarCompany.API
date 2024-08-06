using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.DTO
{
    public class UsernotokenDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string LastName { get; set; }
        
        [Required]

        public AddressDto AddressDto { get; set; }

        [Required]

        public VehicleDto VehicleDto { get; set; }

        [Required]

        public string Phone { get; set; }

        [Required]
        
        public IList<string> roles { get; set; }

       

    }
}
