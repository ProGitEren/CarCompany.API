using Infrastucture.DTO.DTO_Orders;
using Infrastucture.DTO.DTO_OrderVehicles;
using Infrastucture.Params;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Interface.Repository_Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order, int?>
    {
        Task<Tuple<Order, List<string>>> AddAsync(CreateOrderDto dto, Func<CreateOrderVehicleDto,Task<Tuple<OrderVehicle,List<string>>>> orderVehicleFunc);

        Task<Tuple<Order, List<string>>> UpdateAsync(UpdateOrderDto dto, Func<UpdateOrderVehicleDto, Task<Tuple<OrderVehicle, List<string>>>> orderVehicleFunc);

        Task<bool> DeleteAsync(int? Id, Func<int?, Task<bool>> orderVehicleFunc);

        Task<ParamsOrderDto> GetAllAsync(OrderParams orderParams);

    }
}
