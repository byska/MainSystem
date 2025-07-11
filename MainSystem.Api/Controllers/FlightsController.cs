using MainSystem.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MainSystem.Api.Controllers
{
    [Route("api/flights")]
    [ApiController]
    public sealed class FlightsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public FlightsController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> List(
            [FromQuery] DateOnly? date,
            [FromQuery] string? sourceCode,
            [FromQuery] string? destCode,
            [FromQuery] AircraftType? aircraft,
            [FromQuery] bool? shared,
            CancellationToken ct)
        {
            var flights = await _mediator.Send(new ListFlightsQuery(
                                              date, sourceCode, destCode,
                                              aircraft, shared), ct);
            return Ok(flights);
        }

        [HttpGet("{flightNo}")]
        public async Task<IActionResult> Get(string flightNo, CancellationToken ct)
        {
            var flight = await _mediator.Send(new GetFlightQuery(flightNo), ct);
            return flight is null ? NotFound() : Ok(flight);
        }
    }
}
