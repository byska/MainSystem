using MainSystem.Domain.Entities;
using MainSystem.Infrastructure.External.Adapters.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Infrastructure.External.Adapters
{
    public interface ICabinCrewAdapter
    {
        Task<IReadOnlyList<CabinAttendantDto>> ListAllAsync(
        CancellationToken ct = default);
        Task<CabinAttendantDto> GetByIdAsync(Guid id, CancellationToken ct = default);

    }
}
