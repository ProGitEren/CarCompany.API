using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Interface
{
    public interface IGenericRepository <T> where T : class
    {

        IEnumerable<T> GetAll();
        Task AddAsync(T Entity);
        Task DeleteAsync(Guid? AddressId);

        Task UpdateAsync(T Entity);

        Task<T> GetByIdAsync(Guid? Id);

    }
}
