using MainSystem.Application.Abstraction;
using MainSystem.Domain.Entities;
using MainSystem.Domain.Enums;
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
    public sealed class CabinCrewRepository : ICabinCrewPoolRepository
    {
        private readonly ICabinCrewAdapter _adapter;
        public CabinCrewRepository(ICabinCrewAdapter adapter) => _adapter = adapter;

        public async Task<CabinAttendantMember?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var dto = await _adapter.GetByIdAsync(id, ct);
            return dto is null ? null : Map(dto);
        }

        public async Task<IReadOnlyList<CabinAttendantMember>> ListAsync(ISpecification<CabinAttendantMember>? spec = null, CancellationToken ct = default)
        {
            var domain = (await _adapter.ListAllAsync(ct)).Select(Map).ToList();
            return spec is null ? domain : domain.Where(spec.IsSatisfiedBy).ToList();
        }

        private static CabinAttendantMember Map(CabinAttendantDto d)
        {
            var vr = d.VehicleRestrictions
                       .Select(v => Enum.Parse<AircraftType>(v, ignoreCase: true))
                       .ToList();

            return new CabinAttendantMember(
                d.Id,
                d.Info,
                d.AttendantType,
                vr,
                d.Recipes ?? []);
        }
    }
}
