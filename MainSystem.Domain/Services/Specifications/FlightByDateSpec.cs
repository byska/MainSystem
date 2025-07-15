using MainSystem.Domain.Entities;
using MainSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Domain.Services.Specifications
{
    public sealed class FlightByDateSpec : ISpecification<Flight>
    {
        private readonly DateOnly _date;
        public FlightByDateSpec(DateOnly date) => _date = date;

        public bool IsSatisfiedBy(Flight f) =>
            DateOnly.FromDateTime(f.DepartureTime) == _date;

        public string? ErrorMessage => null;
    }

    public sealed class FlightByAircraftSpec : ISpecification<Flight>
    {
        private readonly AircraftType _aircraft;
        public FlightByAircraftSpec(AircraftType t) => _aircraft = t;
        public bool IsSatisfiedBy(Flight f) => f.Aircraft == _aircraft;
        public string? ErrorMessage => null;
    }
}
