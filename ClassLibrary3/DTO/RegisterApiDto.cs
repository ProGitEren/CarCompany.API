﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.DTO
{
    using ClassLibrary2.Entities;
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
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [MaxLength(25, ErrorMessage = " Maximum 25 characters")]
            public string FirstName { get; set; }

            [Required]
            [MaxLength(25, ErrorMessage = " Maximum 25 characters")]
            public string LastName { get; set; }

            [Required]
            public DateTime birthtime { get; set; }

            [Required]
            public string Password { get; set; }

            [Required]
            public RegisterAddressDto Address { get; set; }

            [Required]
            public string Role { get; set; }



        }
    }

}