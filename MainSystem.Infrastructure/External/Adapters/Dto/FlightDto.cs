using MainSystem.Domain.Enums;
using MainSystem.Domain.Services.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Infrastructure.External.Adapters.Dto
{
    public sealed record FlightDto(
     Guid Id,
     string FlightNumber,   
     DateTime FlightDateTime,
     double DurationMinutes,
     double DistanceKm,

     /* ------- Route ------- */
     AirportDto SourceAirport,
     AirportDto DestinationAirport,
     string RouteType,

     AircraftType AircraftType,
    int SeatCount,
    int MaxPassengers,
    int MaxCrew,
    string StandardMenu,

    
    string? PartnerFlightNumber,
    string? PartnerCompany,
    ConnectingDto? ConnectingFlight);

    public sealed record AirportDto(
    string Country,
    string City,
    string Name,
    string Code);                    

    public sealed record ConnectingDto(
        string Destination,
        DateTime DepartureTime);
}
