using ClassLibrary2.Entities;
using Infrastucture.Data;
using Infrastucture.Interface.Repository_Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Repository
{
    public class AddressRepository : GenericRepository<Address, Guid?>, IAddressRepository
    {
        
        public AddressRepository(ApplicationDbContext context) : base(context)
        {

        }

      

    }
}
