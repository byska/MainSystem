using MainSystem.Application.Abstraction;
using MainSystem.Domain.Entities;
using MainSystem.Domain.Services.Specifications;
using MainSystem.Infrastructure.External.Adapters;
using MainSystem.Infrastructure.External.Adapters.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Infrastructure.Repositories
{
    public class PilotPoolRepository : IPilotPoolRepository
    {
        private readonly IPilotPoolAdapter _adapter;
        public PilotPoolRepository(IPilotPoolAdapter adapter) => _adapter = adapter;


        public async  Task<PilotMember?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var dto = await _adapter.GetByIdAsync(id, ct);
            return dto is null ? null : Map(dto);
        }

        public async Task<IReadOnlyList<PilotMember>> ListAsync(ISpecification<PilotMember>? spec = null, CancellationToken ct = default)
        {
            var domain = (await _adapter.ListAllAsync(ct)).Select(Map).ToList();
            return spec is null ? domain : domain.Where(spec.IsSatisfiedBy).ToList();
        }

        private static PilotMember Map(PilotDto d) =>
        new PilotMember(
            d.Id,
            d.Info,
            d.SeniorityLevel,
            d.VehicleRestriction,
            d.AllowedRangeKm);
    }
}
