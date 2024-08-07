using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.DTO
{
    using ClassLibrary2.Entities;
    using global::Infrastucture.DTO.Dto_Address;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace Infrastucture.DTO
    {
        public class RegisterApiDto
        {
          
            public string Email { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public DateTime birthtime { get; set; }

            public string Password { get; set; }

            public RegisterAddressDto Address { get; set; }

            public string Role { get; set; }



        }
    }

}
