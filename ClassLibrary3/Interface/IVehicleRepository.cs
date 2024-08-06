using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Interface
{
    public interface IVehicleRepository : IGenericRepository<Vehicles>
    {
        Task DeleteAsync(string? Id);
    }
}
