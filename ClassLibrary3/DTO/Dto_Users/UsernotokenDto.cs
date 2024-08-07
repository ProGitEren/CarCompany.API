using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastucture.DTO.Dto_Address;
using Infrastucture.DTO.Dto_Vehicles;

namespace Infrastucture.DTO.Dto_Users
{
    public class UsernotokenDto
    {

        public string FirstName { get; set; }

        public string Email { get; set; }

        public string LastName { get; set; }

        public AddressDto AddressDto { get; set; }

        public ICollection<VehicleDto> VehiclesDto { get; set; }

        public string Phone { get; set; }

        public IList<string> roles { get; set; }



    }
}
