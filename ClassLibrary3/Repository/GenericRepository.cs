using Infrastucture.Data;
using Infrastucture.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        private readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T Entity)
        {
            await _context.Set<T>().AddAsync(Entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid? AddressId)
        {
            var entity = await _context.Set<T>().FindAsync(AddressId);
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll()

         => _context.Set<T>().AsNoTracking().ToList();

        public async Task UpdateAsync(T Entity)
        {
            
            if (Entity != null)
            {
                _context.Update<T>(Entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<T> GetByIdAsync(Guid? Id)
        {

                var obj = await _context.FindAsync<T>(Id);
                return obj;
            
        }

    }
}
