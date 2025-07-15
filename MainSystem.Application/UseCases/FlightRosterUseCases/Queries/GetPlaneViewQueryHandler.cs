using MainSystem.Application.Abstraction;
using MainSystem.Domain.Enums;
using MainSystem.Domain.Services.Factories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Application.UseCases.FlightRosterUseCases.Queries
{
    public sealed record GetPlaneViewQuery(Guid RosterId)
        : IRequest<IReadOnlyList<SeatViewResponse>>;
    public sealed record SeatViewResponse(
    int Row,
    char Column,
    SeatClass SeatClass,
    bool Occupied,
    string? PassengerName);

    public class GetPlaneViewQueryHandler : IRequestHandler<GetPlaneViewQuery, IReadOnlyList<SeatViewResponse>>
    {
        private readonly IFlightRosterRepository _rosterRepo;

        public GetPlaneViewQueryHandler(IFlightRosterRepository rosterRepo) =>
            _rosterRepo = rosterRepo;

        public async Task<IReadOnlyList<SeatViewResponse>> Handle(GetPlaneViewQuery request, CancellationToken cancellationToken)
        {
            var roster = await _rosterRepo.GetByIdAsync(request.RosterId, cancellationToken);
            if (roster is null) return Array.Empty<SeatViewResponse>();

            var plan = SeatPlanFactory.Create(roster.Flight.Aircraft);

            var passengers = roster.Passengers.Where(p => p.SeatNumber is not null).ToDictionary(p => p.SeatNumber!, p => p);

            var list = new List<SeatViewResponse>(plan.TotalSeats);

            foreach (var seat in plan.AllSeats())
            {
                passengers.TryGetValue(seat, out var pass);
                list.Add(new SeatViewResponse(
                    seat.Row,
                    seat.Column,
                    plan.GetClass(seat),
                    pass is not null,
                    pass?.Info.Name));
            }

            return list;
        }
    }
}
