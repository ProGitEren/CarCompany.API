using AutoMapper;
using Infrastucture.DTO.DTO_OrderVehicles;
using Infrastucture.DTO.Dto_VehicleModels;
using Infrastucture.Helpers;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Map
{
    public class MappingOrderVehicle : Profile
    {

        public MappingOrderVehicle()
        {
            CreateMap<OrderVehicle, OrderVehicleDto>().
            ForMember(d => d.PicturePaths, o => o.MapFrom<OrderVehicleUrlResolver>())
            .ReverseMap();

            CreateMap<CreateOrderVehicleDto,OrderVehicle>().ReverseMap();
            CreateMap<UpdateOrderVehicleDto,OrderVehicle>().ReverseMap();
        }
    }
}
