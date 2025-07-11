using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace MainSystem.Api.Controllers
{
    [Route("api/rosters")]
    [ApiController]
    public sealed class RostersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RostersController(IMediator mediator) => _mediator = mediator;

        [HttpPost("{flightNo}")]
        public async Task<IActionResult> Create(string flightNo, CancellationToken ct)
        {
            var rosterId = await _mediator.Send(new CreateRosterCommand(flightNo), ct);
            return CreatedAtAction(nameof(Get), new { id = rosterId }, null);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken ct)
        {
            var roster = await _mediator.Send(new GetRosterQuery(id), ct);
            return roster is null ? NotFound() : Ok(roster);
        }
        [HttpGet("{id:guid}/plane-view")]
        public async Task<IActionResult> PlaneView(Guid id, CancellationToken ct)
        {
            var view = await _mediator.Send(new GetPlaneViewQuery(id), ct);
            return view is null ? NotFound() : Ok(view);
        }

        [HttpGet("{id:guid}/export")]
        public async Task<IActionResult> Export(Guid id, CancellationToken ct)
        {
            var roster = await _mediator.Send(new GetRosterQuery(id), ct);
            if (roster is null) return NotFound();

            var json = JsonSerializer.Serialize(roster,
                         new JsonSerializerOptions { WriteIndented = true });
            return File(Encoding.UTF8.GetBytes(json), "application/json",
                        $"roster-{id}.json");
        }
    }
}
