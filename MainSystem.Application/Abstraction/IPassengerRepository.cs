using MainSystem.Domain.Entities;
using MainSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Application.Abstraction
{
    public interface IPassengerRepository : IReadOnlyRepository<PassengerMember>
    {
        Task<IReadOnlyList<PassengerMember>> ListByFlightAsync(
       FlightNumber flightNo, CancellationToken ct = default);
    }
}
