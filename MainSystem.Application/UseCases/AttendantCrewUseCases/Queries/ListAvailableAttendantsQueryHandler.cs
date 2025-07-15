using MainSystem.Application.Abstraction;
using MainSystem.Application.UseCases.PilotUseCases.Queries;
using MainSystem.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Application.UseCases.AttendantCrewUseCases.Queries
{
    public sealed record ListAvailableAttendantsQueryRequest() : IRequest<IReadOnlyList<CabinAttendantMember>>;
    public class ListAvailableAttendantsQueryHandler : IRequestHandler<ListAvailableAttendantsQueryRequest, IReadOnlyList<CabinAttendantMember>>
    {
        private readonly ICabinCrewPoolRepository _repo;
        public ListAvailableAttendantsQueryHandler(ICabinCrewPoolRepository repo)
        {
            _repo = repo;
        }

        public async Task<IReadOnlyList<CabinAttendantMember>> Handle(ListAvailableAttendantsQueryRequest request, CancellationToken cancellationToken)
        {
            return await _repo.ListAsync(null, cancellationToken);
        }
    }
}
