using Infrastucture.Data;
using Infrastucture.Interface;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Repository
{
    public class VehicleRepository :  GenericRepository<Vehicles> , IVehicleRepository
    {
        private readonly ApplicationDbContext _context;
        public VehicleRepository(ApplicationDbContext context) : base(context)
        {
            
        }

        public async Task DeleteAsync(string? Id)
        {
            var entity = await _context.Set<Vehicles>().FindAsync(Id);
            _context.Set<Vehicles>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
