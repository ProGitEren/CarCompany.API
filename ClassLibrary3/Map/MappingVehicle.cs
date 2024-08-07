using AutoMapper;
using Infrastucture.DTO.Dto_Vehicles;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Map
{
    public class MappingVehicle : Profile
    {

        public MappingVehicle()
        {


            CreateMap<VehicleDto, Vehicles>().ReverseMap();
            CreateMap<RegisterVehicleDto, Vehicles>().ReverseMap();

        }

    }
}
