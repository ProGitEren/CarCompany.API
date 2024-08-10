﻿using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Interface.Repository_Interfaces
{
    public interface IEngineRepository : IGenericRepository<Engines, int?>
    {
        Engines GetByEngineCode(string engineCode);
    }
}
