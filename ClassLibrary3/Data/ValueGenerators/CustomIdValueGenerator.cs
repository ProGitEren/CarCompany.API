using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Data.ValueGenerators
{
    public class CustomIdValueGenerator<T> : ValueGenerator where T : BaseEntity<int>
    {
        public override bool GeneratesTemporaryValues => false;
        private ApplicationDbContext _context;
        private static int _currentMaxId; 
        public CustomIdValueGenerator()
        {
            
        }
        protected override object NextValue(EntityEntry entry)
        {
            _context = (ApplicationDbContext)entry.Context;
            _currentMaxId = (_context.Set<T>().Max(x => x.Id)) > 999 ? _context.Set<T>().Max(x => x.Id) : 999;
            Interlocked.Increment(ref _currentMaxId);
            return _currentMaxId;
        }

       
    }
}
