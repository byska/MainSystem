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
    public sealed record GetAllRostersQueryRequest()
        : IRequest<IReadOnlyList<FlightRoster?>?>;

    public sealed class GetAllRostersQueryHandler : IRequestHandler<GetAllRostersQueryRequest, IReadOnlyList<FlightRoster?>>
    {
        private readonly IFlightRosterRepository _repo;
        public GetAllRostersQueryHandler(IFlightRosterRepository repo) => _repo = repo;

        public Task<IReadOnlyList<FlightRoster?>> Handle(GetAllRostersQueryRequest request, CancellationToken cancellationToken)
        {
            return _repo.ListAsync(null, cancellationToken);
        }
    }
}
