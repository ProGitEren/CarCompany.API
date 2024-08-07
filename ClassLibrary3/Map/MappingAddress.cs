using AutoMapper;
using ClassLibrary2.Entities;
using Infrastucture.DTO.Dto_Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Map
{
    public class MappingAddress : Profile
    {

        public MappingAddress()
        {

            CreateMap<AddressDto, Address>().ReverseMap();
            CreateMap<RegisterAddressDto, Address>().ReverseMap();


        }
    }
}
