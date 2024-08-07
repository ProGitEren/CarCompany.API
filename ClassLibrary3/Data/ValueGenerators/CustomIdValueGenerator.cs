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
    public class CustomIdValueGenerator<T> : ValueGenerator<int> where T : BaseEntity<int>
    {
        private static int _currentMaxId;
        private readonly ApplicationDbContext _context;
        public override bool GeneratesTemporaryValues => false;

        public CustomIdValueGenerator(ApplicationDbContext context)
        {
            _context = context;
            _currentMaxId = (_context.Set<T>().Max(x => x.Id)) > 999 ? _context.Set<T>().Max(x => x.Id) : 999;
        }
        public override int Next(EntityEntry entry)
        {
            Interlocked.Increment(ref _currentMaxId);
            return _currentMaxId;
        }

        public override ValueTask<int> NextAsync(EntityEntry entry, CancellationToken cancellationToken = default)
        {
            Interlocked.Increment(ref _currentMaxId);
            return new ValueTask<int>(_currentMaxId);
        }
    }
}
