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
    public class EngineRepository : GenericRepository<Engines, int?>, IEngineRepository
    {

        public EngineRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
