using AutoMapper;
using Infrastucture.DTO.Dto_VehicleModels;
using Infrastucture.Helpers;
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

            CreateMap<VehicleModels, VehicleModelDto>().
            ForMember(d => d.ModelPicturePath, o => o.MapFrom<VehicleModelUrlResolver>())
            .ReverseMap();

            CreateMap<UpdateVehicleModelDto, VehicleModels>().ReverseMap();
            CreateMap<RegisterVehicleModelDto, VehicleModels>().ReverseMap();

        }

    }
}
