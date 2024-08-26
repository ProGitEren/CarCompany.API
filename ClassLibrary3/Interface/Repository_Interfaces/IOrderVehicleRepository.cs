using Infrastucture.DTO.DTO_OrderVehicles;
using Infrastucture.Params;
using Microsoft.AspNetCore.Identity;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Interface.Repository_Interfaces
{
    public interface IOrderVehicleRepository : IGenericRepository<OrderVehicle, int?>
    {
        Task<Tuple<OrderVehicle, List<string>>> AddAsync(CreateOrderVehicleDto dto);

        Task<Tuple<OrderVehicle,List<string>>> UpdateAsync(UpdateOrderVehicleDto dto);

        Task<bool> DeleteAsync(int? Id);

        Task<IdentityResult> DeleteFileAsync(DeleteFileDto dto);
        Task<IdentityResult> AddFileAsync(AddFileDto dto);

        Task<ParamsOrderVehicleDto> GetAllAsync(OrderVehicleParams orderVehicleParams);

        
    }
}
