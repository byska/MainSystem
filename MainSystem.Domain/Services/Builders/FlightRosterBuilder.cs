using MainSystem.Domain.Entities;
using MainSystem.Domain.Enums;
using MainSystem.Domain.Services.Factories;
using MainSystem.Domain.Services.Specifications;
using MainSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Domain.Services.Builders
{
    public sealed class FlightRosterBuilder
    {
        private Flight? _flight;
        private readonly List<PilotMember> _pilots = new();
        private readonly List<CabinAttendantMember> _attendants = new();
        private readonly List<PassengerMember> _passengers = new();

        public FlightRosterBuilder ForFlight(Flight flight)
        {
            _flight = flight ?? throw new ArgumentNullException(nameof(flight));
            return this;
        }


        public FlightRosterBuilder WithPilots(IEnumerable<PilotMember> pilots)
        {
            _pilots.AddRange(pilots);
            return this;
        }

        public FlightRosterBuilder WithCabinCrew(IEnumerable<CabinAttendantMember> attendants)
        {
            _attendants.AddRange(attendants);
            return this;
        }

        public FlightRosterBuilder WithPassengers(IEnumerable<PassengerMember> passengers)
        {
            _passengers.AddRange(passengers);
            return this;
        }

        private void Validate()
        {
            if (_flight is null)
                throw new InvalidOperationException("Flight bilgisi set edilmemiş.");

            var pilotRule = new PilotSenioritySpec();
            var cabinRule = new CabinCrewCountSpec();

            if (!pilotRule.IsSatisfiedBy(_pilots))
                throw new InvalidOperationException(pilotRule.ErrorMessage!);

            if (!cabinRule.IsSatisfiedBy(_attendants))
                throw new InvalidOperationException(cabinRule.ErrorMessage!);

            if (_passengers.Count > _flight.MaxPassengers)
                throw new InvalidOperationException("Yolcu kapasitesi aşıldı.");
        }

        public FlightRoster Build()
        {
            Validate();

            var roster = new FlightRoster(_flight!);

            _pilots.ForEach(roster.AddPilot);
            _attendants.ForEach(roster.AddAttendant);
            _passengers.ForEach(roster.AddPassenger);

            roster.AssignSeats();

            return roster;
        }
    }
}
