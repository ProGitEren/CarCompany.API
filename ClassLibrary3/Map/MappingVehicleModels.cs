﻿using AutoMapper;
using Infrastucture.DTO.Dto_VehicleModels;
using Infrastucture.DTO.Dto_Vehicles;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Map
{
    public class MappingVehicleModels  :   Profile
    {
        public MappingVehicleModels()
        {
            
            CreateMap<VehicleModelDto,VehicleModels>().ReverseMap();
            CreateMap<RegisterVehicleModelDto, VehicleModels>().ReverseMap();

        }

    }
}