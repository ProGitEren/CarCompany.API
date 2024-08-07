using ClassLibrary2.Entities;
using Infrastucture.Data;
using Infrastucture.Interface.Repository_Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Repository
{
    public class VehicleRepository :  GenericRepository<Vehicles,string?> , IVehicleRepository 
    {
     
        public VehicleRepository(ApplicationDbContext context) : base(context)
        {
            
        }

    }
}
