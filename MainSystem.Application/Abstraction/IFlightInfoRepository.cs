using MainSystem.Domain.Entities;
using MainSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Application.Abstraction
{
    public interface IFlightInfoRepository : IReadOnlyRepository<Flight>
    {
        Task<Flight> GetByFlightAsync(
FlightNumber flightNo, CancellationToken ct = default);
    }
}
