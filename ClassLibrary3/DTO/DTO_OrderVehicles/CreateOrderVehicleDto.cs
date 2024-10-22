﻿using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.DTO.DTO_OrderVehicles
{
    public class CreateOrderVehicleDto
    {
        [JsonIgnore]
        public List<IFormFile> Images { get; set; }

        public string VehicleId { get; set; }

        public string ModelName { get; set; }

        public decimal Price { get; set; }
    }
}
