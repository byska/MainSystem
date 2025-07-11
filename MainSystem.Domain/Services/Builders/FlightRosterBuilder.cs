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
        private FlightNumber? _flightNo;
        private DateTime? _departure;
        private AircraftType? _aircraft;

        private readonly List<PilotMember> _pilots = new();
        private readonly List<CabinAttendantMember> _attendants = new();
        private readonly List<PassengerMember> _passengers = new();

        public FlightRosterBuilder ForFlight(FlightNumber flightNo, DateTime departure, AircraftType aircraft)
        {
            _flightNo = flightNo;
            _departure = departure;
            _aircraft = aircraft;
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
            var pilotRule = new PilotSenioritySpec();
            var cabinRule = new CabinCrewCountSpec();

            if (!pilotRule.IsSatisfiedBy(_pilots))
                throw new InvalidOperationException(pilotRule.ErrorMessage!);

            if (!cabinRule.IsSatisfiedBy(_attendants))
                throw new InvalidOperationException(cabinRule.ErrorMessage!);
        }

        public FlightRoster Build()
        {
            Validate();
            if (_flightNo is null ||
                _departure is null ||
                _aircraft is null)
                throw new InvalidOperationException("FlightRosterBuilder: uçuş numarası, kalkış zamanı ve uçak tipi zorunludur.");

            var roster = new FlightRoster(_flightNo, _departure.Value, _aircraft.Value);

            if (!_pilots.Any(p => p.Seniority == PilotSeniorityLevel.Senior) ||
    !_pilots.Any(p => p.Seniority == PilotSeniorityLevel.Junior))
                throw new InvalidOperationException("Uçuşta en az 1 senior ve 1 junior pilot olmalıdır.");

            _pilots.ForEach(roster.AddMember);
            _attendants.ForEach(roster.AddMember);
            _passengers.ForEach(roster.AddMember);

            ISeatPlan seatPlan = SeatPlanFactory.Create(_aircraft.Value);
            Strategies.ISeatAssignmentStrategy strategy = SeatAssignmentStrategyFactory.Create(seatPlan);

            strategy.AssignSeats(roster);

            return roster;
        }
    }
}
