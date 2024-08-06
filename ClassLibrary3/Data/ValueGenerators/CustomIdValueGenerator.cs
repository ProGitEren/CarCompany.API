using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Data.ValueGenerators
{
    public class CustomIdValueGenerator : ValueGenerator<int>
    {
        private int _current = 999;

        public override bool GeneratesTemporaryValues => false;

        public override int Next(EntityEntry entry)
        {
            Interlocked.Increment(ref _current);
            return _current;
        }

        public override ValueTask<int> NextAsync(EntityEntry entry, CancellationToken cancellationToken = default)
        {
            Interlocked.Increment(ref _current);
            return new ValueTask<int>(_current);
        }
    }
}
