using MainSystem.Domain.Enums;
using MainSystem.Infrastructure.External.Adapters.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Infrastructure.External.Adapters
{
    public interface IPilotPoolAdapter
    {
        Task<IReadOnlyList<PilotDto>> ListAllAsync(CancellationToken ct = default);
        Task<PilotDto> GetByIdAsync(Guid id, CancellationToken ct = default);
    }
}
