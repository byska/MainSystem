using MainSystem.Application.Abstraction;
using MainSystem.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Application.UseCases.FlightRosterUseCases.Queries
{
    public sealed record GetRosterByIdQueryRequest(Guid RosterId)
        : IRequest<FlightRoster?>;

    public sealed class GetRosterByIdQueryHandler : IRequestHandler<GetRosterByIdQueryRequest, FlightRoster?>
    {
        private readonly IFlightRosterRepository _repo;

        public GetRosterByIdQueryHandler(IFlightRosterRepository repo) => _repo = repo;

        public Task<FlightRoster?> Handle(GetRosterByIdQueryRequest q, CancellationToken ct)
        {
            return _repo.GetByIdAsync(q.RosterId, ct);
        }
    }

}
