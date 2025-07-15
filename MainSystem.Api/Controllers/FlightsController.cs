using MainSystem.Api.Models;
using MainSystem.Application.UseCases.AttendantCrewUseCases.Queries;
using MainSystem.Application.UseCases.FlightRosterUseCases.Queries;
using MainSystem.Application.UseCases.FlightUseCases.Queries;
using MainSystem.Application.UseCases.PilotUseCases.Queries;
using MainSystem.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MainSystem.Api.Controllers
{

    public sealed class FlightsController : Controller
    {
        private readonly IMediator _mediator;
        public FlightsController(IMediator mediator) => _mediator = mediator;
        public async Task<IActionResult> Home(CancellationToken cancellationToken)
        {
            var activeFlights = await _mediator.Send(new ListFlightsQueryRequest(
    null,
    DateOnly.FromDateTime(DateTime.UtcNow.Date),
    null, null, null, null), cancellationToken);

            var activeFlightCount = activeFlights.Count;

            var activePilots = await _mediator.Send(new ListAvailablePilotsQueryRequest());
            var activePilotsCount = activePilots.Count;

            var activeAttendants = await _mediator.Send(new ListAvailableAttendantsQueryRequest());
            var activeAttendantsCount = activeAttendants.Count;

            var activeRosters = await _mediator.Send(new GetAllRostersQueryRequest());
            var activeRostersCount = activeRosters.Count;

            var dashboard = new DashboardViewModel
            {
                ActiveFlightCount = activePilotsCount,
                AvailableCrewCount = activeRostersCount,
                AvailablePilotCount =activePilotsCount,
                RosterCount = activeRostersCount
            };

            var model = new FlightIndexViewModel
            {
                Dashboard = dashboard,
                Search = new SearchFlightViewModel()
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> SearchFlights(
           FlightIndexViewModel model,
            CancellationToken ct = default)
        {
            var flights = await _mediator.Send(new ListFlightsQueryRequest(
               model.Search.FlightNumber,
                model.Search.Date,
                model.Search.SourceCode,
                model.Search.DestCode,
                model.Search.Aircraft,
                false
            ), ct);

            return RedirectToAction("Index", "Rosters", flights);
        }
        //[HttpGet]
        //public async Task<IActionResult> List(
        //    [FromQuery] DateOnly? date,
        //    [FromQuery] string? sourceCode,
        //    [FromQuery] string? destCode,
        //    [FromQuery] AircraftType? aircraft,
        //    [FromQuery] bool? shared,
        //    CancellationToken ct)
        //{
        //    var flights = await _mediator.Send(new ListFlightsQueryRequest(
        //                                      date, sourceCode, destCode,
        //                                      aircraft, shared), ct);
        //    return Ok(flights);
        //}

        //[HttpGet("{flightNo}")]
        //public async Task<IActionResult> GetByFlightNo(string flightNo, CancellationToken ct)
        //{
        //    var flight = await _mediator.Send(new GetByFlightNoQueryRequest(flightNo), ct);
        //    return flight is null ? NotFound() : Ok(flight);
        //}
    }
}
