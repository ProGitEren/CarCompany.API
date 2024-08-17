using Infrastucture.Data;
using Infrastucture.DTO.Dto_VehicleModels;
using Infrastucture.DTO.Dto_Vehicles;
using Infrastucture.Params;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Interface.Repository_Interfaces
{
    public interface IVehicleRepository : IGenericRepository<Vehicles, string?>
    {
        Task<ParamsVehicleDto> GetAllAsync(VehicleParams vehicleParams);

    }


}
