using MainSystem.Application.Abstraction;
using MainSystem.Domain.Entities;
using MainSystem.Domain.Services.Specifications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Application.UseCases.PilotUseCases.Queries
{
    public sealed record ListAvailablePilotsQueryRequest(): IRequest<IReadOnlyList<PilotMember>>;

    public class ListAvailablePilotsQueryHandler : IRequestHandler<ListAvailablePilotsQueryRequest, IReadOnlyList<PilotMember>>
    {
        private readonly IPilotPoolRepository _repo;

        public ListAvailablePilotsQueryHandler(IPilotPoolRepository repo)
        {
            _repo = repo;
        }

        public async Task<IReadOnlyList<PilotMember>> Handle(ListAvailablePilotsQueryRequest request, CancellationToken cancellationToken)
        {
            return await _repo.ListAsync(null, cancellationToken);
        }
    }
}
