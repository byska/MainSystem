using MainSystem.Application.Abstraction;
using MainSystem.Domain.Entities;
using MainSystem.Domain.Services.Specifications;
using MainSystem.Domain.ValueObjects;
using MainSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Infrastructure.Repositories
{
    public class FlightRosterRepository : IFlightRosterRepository
    {
        private readonly MainSystemDbContext _dbContext;

        public FlightRosterRepository(MainSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(FlightRoster roster, CancellationToken ct = default)
        {
           await _dbContext.Rosters.AddAsync(roster); 
        }

        public async Task<FlightRoster?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _dbContext.Rosters.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<FlightRoster>> ListAsync(ISpecification<FlightRoster>? spec = null, CancellationToken ct = default)
        {
           List<FlightRoster> rosters = await _dbContext.Rosters.ToListAsync();
            return spec is null ? rosters : rosters.Where(spec.IsSatisfiedBy).ToList();
        }

        public async Task<IReadOnlyList<FlightRoster>> ListByFlightNoAsync(FlightNumber flightNo, CancellationToken ct = default)
        {
            return await _dbContext.Rosters.Include(x=>x.Flight)
             .Where(r => r.Flight.FlightNo == flightNo)
             .ToListAsync(ct);
        }
    }
}
