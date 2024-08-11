using Infrastucture.Data;
using Infrastucture.Interface.Repository_Interfaces;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Repository
{
    public class VehicleModelRepository : GenericRepository<VehicleModels, int?>, IVehicleModelRepository
    {
        
        public VehicleModelRepository(ApplicationDbContext context) : base(context)
        {

        }

        public VehicleModels GetModelByCode(string ModelCode)
        {
            var model = this.GetAll().FirstOrDefault(x => x.ModelCode == ModelCode);
            return model;
        }


    }
}
