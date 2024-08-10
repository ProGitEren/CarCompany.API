using AutoMapper;
using ClassLibrary2.Entities;
using Infrastucture.DTO.Dto_Address;
using Infrastucture.DTO.Dto_Users;
using Infrastucture.DTO.Dto_Vehicles;
using Models.Entities;
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


            // Mapping UserwithdetailsDto to AppUsers and vice versa
            CreateMap<UserwithdetailsDto, AppUsers>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.AddressDto))
                .ForMember(dest => dest.Vehicles, opt => opt.MapFrom(src => src.VehiclesDto.Select(vehicleDto => new Vehicles
                {
                    Vin = vehicleDto.Vin,
                    Averagefuelin = vehicleDto.Averagefuelin,
                    Averagefuelout = vehicleDto.Averagefuelout,
                    COemmission = vehicleDto.COemmission,
                    FuelCapacity = vehicleDto.FuelCapacity,
                    MaxAllowedWeight = vehicleDto.MaxAllowedWeight,
                    MinWeight = vehicleDto.MinWeight,
                    BaggageVolume = vehicleDto.BaggageVolume,
                    DrivenKM = vehicleDto.DrivenKM,
                    ModelId = vehicleDto.ModelId,
                    EngineId = vehicleDto.EngineId,
                    UserId = vehicleDto.UserId,
                }).ToList()))
                .ReverseMap()
                .ForMember(dest => dest.AddressDto, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.VehiclesDto, opt => opt.MapFrom(src => src.Vehicles.Select(vehicle => new VehicleDto
                {
                    Vin = vehicle.Vin,
                    Averagefuelin = vehicle.Averagefuelin,
                    Averagefuelout = vehicle.Averagefuelout,
                    COemmission = vehicle.COemmission,
                    FuelCapacity = vehicle.FuelCapacity,
                    MaxAllowedWeight = vehicle.MaxAllowedWeight,
                    MinWeight = vehicle.MinWeight,
                    BaggageVolume = vehicle.BaggageVolume,
                    DrivenKM = vehicle.DrivenKM,
                    ModelId = vehicle.ModelId,
                    EngineId = vehicle.EngineId,
                    UserId = vehicle.UserId

                }).ToList()));




            CreateMap<UsernotokenDto, AppUsers>()
                 .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.AddressDto))
                 .ForMember(dest => dest.Vehicles, opt => opt.MapFrom(src => src.VehiclesDto.Select(vehicleDto => new Vehicles
                 {
                     Vin = vehicleDto.Vin,
                     Averagefuelin = vehicleDto.Averagefuelin,
                     Averagefuelout = vehicleDto.Averagefuelout,
                     COemmission = vehicleDto.COemmission,
                     FuelCapacity = vehicleDto.FuelCapacity,
                     MaxAllowedWeight = vehicleDto.MaxAllowedWeight,
                     MinWeight = vehicleDto.MinWeight,
                     BaggageVolume = vehicleDto.BaggageVolume,
                     DrivenKM = vehicleDto.DrivenKM,
                     ModelId = vehicleDto.ModelId,
                     EngineId = vehicleDto.EngineId
                 }).ToList()))
                 .ReverseMap()
                 .ForMember(dest => dest.AddressDto, opt => opt.MapFrom(src => src.Address))
                 .ForMember(dest => dest.VehiclesDto, opt => opt.MapFrom(src => src.Vehicles.Select(vehicle => new VehicleDto
                 {
                     Vin = vehicle.Vin,
                     Averagefuelin = vehicle.Averagefuelin,
                     Averagefuelout = vehicle.Averagefuelout,
                     COemmission = vehicle.COemmission,
                     FuelCapacity = vehicle.FuelCapacity,
                     MaxAllowedWeight = vehicle.MaxAllowedWeight,
                     MinWeight = vehicle.MinWeight,
                     BaggageVolume = vehicle.BaggageVolume,
                     DrivenKM = vehicle.DrivenKM,
                     ModelId = vehicle.ModelId,
                     EngineId = vehicle.EngineId
                 }).ToList()));


        }

        // Custom type converters if needed
       
    }
}
