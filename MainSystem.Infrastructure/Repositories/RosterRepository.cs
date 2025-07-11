using MainSystem.Application.Abstraction;
using MainSystem.Domain.Entities;
using MainSystem.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Infrastructure.Repositories
{
    public sealed class RosterRepository : ReadOnlyRepository<FlightRoster>, IRosterRepository
    {
        private readonly MainSystemDbContext _db;

        public RosterRepository(MainSystemDbContext db) : base(db) => _db = db;

        public async Task AddAsync(FlightRoster entity, CancellationToken ct = default)
        {
            await _db.Rosters.AddAsync(entity, ct).AsTask();
        }
    }
}
