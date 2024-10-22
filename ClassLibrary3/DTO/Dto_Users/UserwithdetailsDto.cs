using ClassLibrary2.Entities;
using Infrastucture.DTO.Dto_Address;
using Infrastucture.DTO.Dto_Vehicles;
using Models.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.DTO.Dto_Users
{
    public class UserwithdetailsDto 
    {
        public string FirstName { get; set; }

        public string Email { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public AddressDto AddressDto { get; set; }

        public List<VehicleDto> VehiclesDto { get; set; }

        public string Token { get; set; }

        public IList<string> roles { get; set; }


    }
}
