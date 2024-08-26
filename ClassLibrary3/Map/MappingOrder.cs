using AutoMapper;
using Infrastucture.DTO.DTO_Orders;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Map
{
    public class MappingOrder : Profile
    {
        public MappingOrder()
        {
            CreateMap<Order, OrderDto>()
                .ReverseMap();

            CreateMap<Order, CreateOrderDto>()
                .ForMember(x => x.OrderVehicleDto, src => src.Ignore()).ReverseMap();
            
            CreateMap<Order, UpdateOrderDto>()
                .ForMember(x => x.OrderVehicleDto, src => src.Ignore()).ReverseMap();

        }
    }
}
