using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Interface.Repository_Interfaces
{
    public interface IGenericRepository<T, K> where T : class
    {

        IEnumerable<T> GetAll();
        Task AddAsync(T Entity);

        Task UpdateAsync(T Entity);

        Task DeleteAsync(K Id);


        Task<T> GetByIdAsync(K Id);


    }
}
