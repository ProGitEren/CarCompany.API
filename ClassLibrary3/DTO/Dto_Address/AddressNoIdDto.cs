﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.DTO.Dto_Address
{
    public class AddressNoIdDto
    {

        public string name { get; set; }

        public string city { get; set; }

        public string state { get; set; }

        public int zipcode { get; set; }

        public string country { get; set; }

    }
}
