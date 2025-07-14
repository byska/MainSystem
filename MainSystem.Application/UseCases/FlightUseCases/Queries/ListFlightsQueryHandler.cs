using MainSystem.Application.Abstraction;
using MainSystem.Domain.Entities;
using MainSystem.Domain.Enums;
using MainSystem.Domain.Services.Specifications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Application.UseCases.FlightUseCases.Queries
{
    public sealed record ListFlightsQueryRequest(
    DateOnly? Date,
    string? SourceCode,
    string? DestCode,
    AircraftType? Aircraft,
    bool? Shared) : IRequest<IReadOnlyList<FlightRoster>>;

    public class ListFlightsQueryHandler : IRequestHandler<ListFlightsQueryRequest, IReadOnlyList<FlightRoster>>
    {
        private readonly IFlightInfoRepository _repo;
        public ListFlightsQueryHandler(IFlightInfoRepository repo) => _repo = repo;

        public async Task<IReadOnlyList<FlightRoster>> Handle(
            ListFlightsQueryRequest q, CancellationToken ct)
        {
            CompositeSpecification<FlightRoster> spec = new TrueSpec<FlightRoster>();

            if (q.Date is { } d)
                spec = spec.And(new FlightByDateSpec(d));
            if (!string.IsNullOrWhiteSpace(q.SourceCode))
                spec = spec.And(new FlightBySourceAirportSpec(q.SourceCode!));
            if (!string.IsNullOrWhiteSpace(q.DestCode))
                spec = spec.And(new FlightByDestAirportSpec(q.DestCode!));
            if (q.Aircraft is { } ac)
                spec = spec.And(new FlightByAircraftSpec(ac));
            if (q.Shared is { } sh)
                spec = spec.And(new FlightSharedSpec(sh));

            return await _repo.ListAsync(spec is TrueSpec<FlightRoster> ? null : spec, ct);
        }
    }
}
