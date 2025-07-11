using MainSystem.Infrastructure.External.Adapters.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Infrastructure.External.Adapters
{
    public interface IPassengerAdapter
    {
        Task<IReadOnlyList<PassengerDto>> ListByFlightAsync(string flightNumber, CancellationToken ct = default);

        Task<IReadOnlyList<PassengerDto>> ListAllAsync(CancellationToken ct = default);
        Task<PassengerDto> GetByIdAsync(Guid id, CancellationToken ct = default);
    }
}
