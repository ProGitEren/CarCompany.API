using ClassLibrary2.Entities;
using Infrastucture.Data;
using Infrastucture.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Repository
{
    public class AddressRepository : GenericRepository<Address>, IAddressRepository
    {

        public AddressRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
