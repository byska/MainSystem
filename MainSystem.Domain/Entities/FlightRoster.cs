using MainSystem.Domain.Enums;
using MainSystem.Domain.Services.Factories;
using MainSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Domain.Entities
{
    public class FlightRoster
    {
        public Guid Id { get; private set; }
        public FlightNumber FlightNo { get; private set; }
        public DateTime DepartureTime { get; private set; }
        public FlightDuration Duration { get; private set; }
        public double DistanceKm { get; private set; }

        public Airport SourceAirport { get; private set; }
        public Airport DestinationAirport { get; private set; }
        public RouteType RouteType { get; private set; }

        public AircraftType Aircraft { get; private set; }
        public int SeatCount { get; private set; }
        public int MaxPassengers { get; private set; }
        public int MaxCrew { get; private set; }
        public string StandardMenu { get; private set; }
        public ISeatPlan SeatPlan { get; private set; }

        public bool IsShared => SharedFlight is not null;
        public SharedFlight? SharedFlight { get; private set; }


        private readonly List<RosterMember> _members = [];
        public IReadOnlyCollection<RosterMember> Members => _members.AsReadOnly();

        private FlightRoster() { }

        public FlightRoster(
        FlightNumber flightNo,
        DateTime departure,
        FlightDuration duration,
        double distanceKm,
        Airport source,
        Airport destination,
        RouteType routeType,
        VehicleType vehicle,
        SharedFlight? sharedFlight = null)

        {
            Id = Guid.NewGuid();
            FlightNo = flightNo;
            DepartureTime = departure;
            Duration = duration;
            DistanceKm = distanceKm;

            SourceAirport = source;
            DestinationAirport = destination;
            RouteType = routeType;

            Aircraft = vehicle.AircraftType;
            SeatCount = vehicle.SeatCount;
            MaxPassengers = vehicle.MaxPassengers;
            MaxCrew = vehicle.MaxCrew;
            StandardMenu = vehicle.StandardMenu;

            SeatPlan = SeatPlanFactory.Create(Aircraft);

            SharedFlight = sharedFlight;

        }
        public void AddMember(RosterMember member)
        {
            if (member is null) throw new ArgumentNullException(nameof(member));
            _members.Add(member);
        }

        public IEnumerable<PilotMember> Pilots => _members.OfType<PilotMember>();
        public IEnumerable<CabinAttendantMember> CabinAttendants => _members.OfType<CabinAttendantMember>();
        public IEnumerable<PassengerMember> Passengers => _members.OfType<PassengerMember>();
        public int PassengerCount => Passengers.Count();
        public int CabinCrewCount => CabinAttendants.Count();
        public int FlightCrewCount => Pilots.Count();

        public IEnumerable<SeatNumber> OccupiedSeats() =>
            Passengers.Where(p => p.SeatNumber is not null)
                      .Select(p => p.SeatNumber!);

        public IEnumerable<SeatNumber> AvailableSeats() =>
            SeatPlan.AllSeats().Except(OccupiedSeats());
        public void AddSharedFlight(SharedFlight shared)
        {
            if (IsShared) throw new InvalidOperationException("Shared flight mevcut.");
            SharedFlight = shared ?? throw new ArgumentNullException(nameof(shared));
        }
    }
}
