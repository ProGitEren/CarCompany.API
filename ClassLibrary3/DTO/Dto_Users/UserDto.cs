using ClassLibrary2.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.DTO.Dto_Users
{
    public class UserDto
    {

        public string FirstName { get; set; }

        public string Email { get; set; }

        public string LastName { get; set; }

        public string Token { get; set; }


    }
}
