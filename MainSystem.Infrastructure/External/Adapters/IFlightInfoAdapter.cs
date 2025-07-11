using MainSystem.Infrastructure.External.Adapters.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Infrastructure.External.Adapters
{
    public interface IFlightInfoAdapter
    {
        Task<IReadOnlyList<FlightDto>> ListAllAsync(CancellationToken ct = default);
        Task<FlightDto> GetByIdAsync(Guid id, CancellationToken ct = default);

    }
}
