using AutoMapper;
using AutoMapper.Execution;
using Infrastucture.DTO.Dto_VehicleModels;
using Microsoft.Extensions.Configuration;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Helpers
{
    public class VehicleModelUrlResolver : IValueResolver<VehicleModels, VehicleModelDto, string>
    {
        private readonly IConfiguration _configuration;

        public VehicleModelUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(VehicleModels source, VehicleModelDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ModelPicture))
            {
                return _configuration["API_url"] + source.ModelPicture;
            }
            return null;
        }
    }
}
