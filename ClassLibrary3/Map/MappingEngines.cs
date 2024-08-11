using AutoMapper;
using Infrastucture.DTO.Dto_Engines;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Map
{
    public class MappingEngines : Profile
    {
        public MappingEngines()
        {
            CreateMap<EngineDto,Engines>().ReverseMap();
            CreateMap<RegisterEngineDto,Engines>().ReverseMap();


        }

    }
}
