using MainSystem.Application.Abstraction;
using MainSystem.Domain.Entities;
using MainSystem.Domain.Enums;
using MainSystem.Domain.Services.Specifications;
using MainSystem.Domain.ValueObjects;
using MainSystem.Infrastructure.External.Adapters;
using MainSystem.Infrastructure.External.Adapters.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Infrastructure.Repositories
{
    public class FlightInfoRepository : IFlightInfoRepository
    {
        private readonly IFlightInfoAdapter _adapter;
        public FlightInfoRepository(IFlightInfoAdapter adapter) => _adapter = adapter;
        public async Task<FlightRoster?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var flight = await _adapter.GetByIdAsync(id, ct);
            return flight is null ? null : Map(flight);
        }

        public async Task<IReadOnlyList<FlightRoster>> ListAsync(ISpecification<FlightRoster>? spec = null, CancellationToken ct = default)
        {
            var dtoList = await _adapter.ListAllAsync(ct);
            var domain = dtoList.Select(Map).ToList();
            return spec is null ? domain : domain.Where(spec.IsSatisfiedBy).ToList();
        }
        private static FlightRoster Map(FlightDto d)
        {
            var source = new Airport(
                d.SourceAirport.Country,
                d.SourceAirport.City,
                d.SourceAirport.Name,
                new AirportCode(d.SourceAirport.Code));

            var destination = new Airport(
                d.DestinationAirport.Country,
                d.DestinationAirport.City,
                d.DestinationAirport.Name,
                new AirportCode(d.DestinationAirport.Code));

            var routeType = Enum.Parse<RouteType>(d.RouteType, ignoreCase: true);


            var vehicle = new VehicleType(
        d.AircraftType,
        d.SeatCount,
        d.MaxPassengers,
        d.MaxCrew,
        d.StandardMenu);

            SharedFlight? shared = null;

            if (!string.IsNullOrWhiteSpace(d.PartnerFlightNumber) &&
                !string.IsNullOrWhiteSpace(d.PartnerCompany))
            {
                shared = new SharedFlight(
                    new FlightNumber(d.PartnerFlightNumber),
                    d.PartnerCompany);

                if (d.ConnectingFlight is { } c)
                    shared.AddConnectingFlight(c.Destination, c.DepartureTime);
            }

            var roster = new FlightRoster(
        new FlightNumber(d.FlightNumber),
        d.FlightDateTime,
        new FlightDuration(TimeSpan.FromMinutes(d.DurationMinutes)),
        d.DistanceKm,
        source,
        destination,
        routeType,
        vehicle,
        shared);

            return roster;
        }
    }
}
