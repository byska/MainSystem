using MainSystem.Domain.Entities;
using MainSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Application.Abstraction
{
    public interface IFlightRosterRepository : IReadOnlyRepository<FlightRoster>
    {
        Task<IReadOnlyList<FlightRoster>> ListByFlightNoAsync(
       FlightNumber flightNo, CancellationToken ct = default);

        Task AddAsync(FlightRoster roster, CancellationToken ct = default);
    }
}
