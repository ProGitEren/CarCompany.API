using Infrastucture.DTO.Dto_VehicleModels;
using Infrastucture.Params;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Interface.Repository_Interfaces
{
    public interface IVehicleModelRepository : IGenericRepository<VehicleModels, int?>
    {
      Task<VehicleModels> AddAsync(RegisterVehicleModelDto dto);
      Task<VehicleModels> UpdateAsync(UpdateVehicleModelDto dto);
      Task<bool> DeleteAsync(int? id);
      Task<ParamsModelDto> GetAllAsync(VehiclemodelParams modelParams);
    }

   
}
