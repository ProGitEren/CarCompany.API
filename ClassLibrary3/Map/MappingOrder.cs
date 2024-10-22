using AutoMapper;
using Infrastucture.DTO.DTO_Orders;
using Infrastucture.DTO.DTO_OrderVehicles;
using Models.Entities;
using Newtonsoft.Json;
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

            // Mapping from MultiFormCreateOrderDto back to CreateOrderDto
            CreateMap<MultiFormCreateOrderDto, CreateOrderDto>()
                .ForMember(dest => dest.TransferAddress,
                           opt => opt.MapFrom(src => JsonConvert.DeserializeObject<TransferAddress>(src.TransferAddressJson)))
                .ForMember(dest => dest.OrderVehicleDto,
                           opt => opt.MapFrom(src => JsonConvert.DeserializeObject<CreateOrderVehicleDto>(src.OrderVehicleDtoJson)))
                .AfterMap((src, dest) =>
                {
                    dest.OrderVehicleDto.Images = src.Images; // Assign the images after deserialization
                });

            CreateMap<CreateOrderVehicleDtoWithoutFiles, CreateOrderVehicleDto>();
        }
    }
}
