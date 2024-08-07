using ClassLibrary2.Entities;
using Infrastucture.DTO.Dto_Address;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.DTO.Dto_Users
{
    public class RegisterDto
    {

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public DateTime birthtime { get; set; }

        public string EncryptedPassword { get; set; }

        public RegisterAddressDto Address { get; set; }

        public string Role { get; set; }



    }
}
