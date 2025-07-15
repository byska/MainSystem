using MainSystem.Application.Abstraction;
using MainSystem.Domain.Entities;
using MainSystem.Domain.Enums;
using MainSystem.Domain.Services.Specifications;
using MainSystem.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Application.UseCases.FlightUseCases.Queries
{
    public sealed record ListFlightsQueryRequest(
    string? FlightNumber,
    DateOnly? Date,
    string? SourceCode,
    string? DestCode,
    AircraftType? Aircraft,
    bool? Shared) : IRequest<IReadOnlyList<Flight>>;

    public class ListFlightsQueryHandler : IRequestHandler<ListFlightsQueryRequest, IReadOnlyList<Flight>>
    {
        private readonly IFlightInfoRepository _repo;
        public ListFlightsQueryHandler(IFlightInfoRepository repo) => _repo = repo;

        public async Task<IReadOnlyList<Flight>> Handle(
            ListFlightsQueryRequest q, CancellationToken ct)
        {
            CompositeSpecification<Flight> spec = new TrueSpec<Flight>();
            if (q.FlightNumber != null) {
                var number = new FlightNumber(q.FlightNumber);
                spec = spec.And(new FlightByNumberSpec(number));
            }
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

            return await _repo.ListAsync(spec is TrueSpec<Flight> ? null : spec, ct);
        }
    }
}
