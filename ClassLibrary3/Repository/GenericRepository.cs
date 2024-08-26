using ClassLibrary2.Entities;
using Infrastucture.Data;
using Infrastucture.Interface.Repository_Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Validation;

namespace Infrastucture.Repository
{
    public class GenericRepository< T , K > : IGenericRepository< T , K > where T : class
    {

        private readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T Entity)
        {
            var validationerrorlist = EntityValidator.GetValidationResults(Entity);

            if (!validationerrorlist.Any())
            {
                await _context.Set<T>().AddAsync(Entity);
                await _context.SaveChangesAsync();
                // no database operation
            }
            
        }

        public IEnumerable<T> GetAll()

         => _context.Set<T>().AsNoTracking().ToList();

        public async Task UpdateAsync(T Entity)
        {
            
            if (Entity != null)
            {
                var validationerrorlist = EntityValidator.GetValidationResults(Entity);

                if (!validationerrorlist.Any())
                {
                    _context.Update<T>(Entity);
                    await _context.SaveChangesAsync();
                    // no database operation
                }
              
            }
        }


        public async Task DeleteAsync(K Id)
        {
            var entity = await _context.Set<T>().FindAsync(Id);
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
            
        public async Task<T> GetByIdAsync(K Id)
        {

            var obj = await _context.FindAsync<T>(Id);
            return obj;

        }



    }
}
