using MainSystem.Application.Abstraction;
using MainSystem.Domain.Entities;
using MainSystem.Domain.Services.Specifications;
using MainSystem.Domain.ValueObjects;
using MainSystem.Infrastructure.External.Adapters;
using MainSystem.Infrastructure.External.Adapters.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Infrastructure.Repositories
{
    public sealed class PassengerRepository : IPassengerRepository
    {
        private readonly IPassengerAdapter _adapter;
        public PassengerRepository(IPassengerAdapter adapter) => _adapter = adapter;

        public async Task<PassengerMember?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var passanger = await _adapter.GetByIdAsync(id, ct);
            return passanger is null ? null : Map(passanger);

        }

        public async Task<IReadOnlyList<PassengerMember>> ListAsync(ISpecification<PassengerMember>? spec = null, CancellationToken ct = default)
        {
            var dtoList = await _adapter.ListAllAsync(ct);
            var domain = dtoList.Select(Map).ToList();
            return spec is null ? domain : domain.Where(spec.IsSatisfiedBy).ToList();
        }

        public async Task<IReadOnlyList<PassengerMember>> ListByFlightAsync(FlightNumber flightNo, CancellationToken ct = default)
        {
            var dtoList = await _adapter.ListByFlightAsync(flightNo.Value, ct);
            return dtoList.Select(Map).ToList();
        }

        private PassengerMember Map( PassengerDto dto) =>
         new(dto.Id,
             new FlightNumber(dto.FlightNumber),
             dto.PassengerInfo, 
             dto.IsInfant,
             dto.SeatClass,
             dto.SeatNumber,
             dto.ParentPassengerId,
             dto.AffiliatedPassengerIds ?? Enumerable.Empty<Guid>());

    }
}
