﻿using Infrastucture.DTO.Dto_VehicleModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.DTO.Dto_Vehicles
{
    public class ParamsVehicleDto
    {
        public int TotalItems { get; set; }
        public int PageItemCount { get; set; }

        public List<VehicleDto> VehicleDtos { get; set; }

    }
}