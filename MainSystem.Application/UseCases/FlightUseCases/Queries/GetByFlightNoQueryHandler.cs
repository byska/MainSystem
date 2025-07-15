using MainSystem.Application.Abstraction;
using MainSystem.Domain.Entities;
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
    public sealed record GetByFlightNoQueryRequest(string FlightNumber)
    : IRequest<Flight?>;
    public class GetByFlightNoQueryHandler : IRequestHandler<GetByFlightNoQueryRequest, Flight?>
    {
        private readonly IFlightInfoRepository _repo;

        public GetByFlightNoQueryHandler(IFlightInfoRepository repo) =>
            _repo = repo;

        public async Task<Flight?> Handle(GetByFlightNoQueryRequest request, CancellationToken cancellationToken)
        {
            var number = new FlightNumber(request.FlightNumber);

            // 2) spesifikasyon ile tek kaydı filtrele
            var spec = new FlightByNumberSpec(number);      // → ExpressionSpecification

            var entity = (await _repo.ListAsync(spec, cancellationToken)).FirstOrDefault();
            if (entity is null) return null;

            return entity;
        }
    }
}

