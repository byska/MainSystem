using MainSystem.Domain.Enums;
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
        public AircraftType Aircraft { get; private set; }

        private readonly List<RosterMember> _members = [];
        public IReadOnlyCollection<RosterMember> Members => _members.AsReadOnly();

        private FlightRoster() { }

        public FlightRoster(FlightNumber flightNo,
                            DateTime departureTime,
                            AircraftType aircraft)
        {
            Id = Guid.NewGuid();
            FlightNo = flightNo;
            DepartureTime = departureTime;
            Aircraft = aircraft;

        }
        public void AddMember(RosterMember member)
        {
            if (member is null) throw new ArgumentNullException(nameof(member));
            _members.Add(member);
        }

        public IEnumerable<PilotMember> Pilots => _members.OfType<PilotMember>();
        public IEnumerable<CabinAttendantMember> CabinAttendants => _members.OfType<CabinAttendantMember>();
        public IEnumerable<PassengerMember> Passengers => _members.OfType<PassengerMember>();
    }
}
