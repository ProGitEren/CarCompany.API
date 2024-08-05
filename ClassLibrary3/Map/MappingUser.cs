using AutoMapper;
using ClassLibrary2.Entities;
using Infrastucture.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Map
{
    public class MappingUser : Profile
    {
        
        public MappingUser()
        {
            CreateMap<UserDto, AppUsers>().ReverseMap();
            CreateMap<UserDto, UpdateUserDto>().ReverseMap();
            CreateMap<UpdateUserDto, AppUsers>().ReverseMap();
            CreateMap<RegisterDto, AppUsers>().ReverseMap();
            CreateMap<LoginDto, AppUsers>().ReverseMap();

            CreateMap<AddressDto, Address>().ReverseMap();
            CreateMap<RegisterAddressDto, Address>().ReverseMap();
            

            CreateMap<UserwithaddressDto, AppUsers>()
                .ForMember(x => x.Address, opt => opt.MapFrom(src => src.AddressDto))
                .ReverseMap().ForMember(x => x.AddressDto, opt => opt.MapFrom(src => src.Address))
                .ReverseMap();


            CreateMap<UsernotokenDto, AppUsers>()
                .ForMember(x => x.Address, opt => opt.MapFrom(src => src.AddressDto))
                .ReverseMap().ForMember(x => x.AddressDto, opt => opt.MapFrom(src => src.Address))
                .ReverseMap();


        }
    }
}
