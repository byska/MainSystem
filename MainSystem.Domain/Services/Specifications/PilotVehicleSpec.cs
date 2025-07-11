using MainSystem.Domain.Entities;
using MainSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Domain.Services.Specifications
{
    public sealed class PilotVehicleSpec : ISpecification<PilotMember>
    {
        private readonly AircraftType _aircraft;
        public PilotVehicleSpec(AircraftType aircraft) => _aircraft = aircraft;

        public bool IsSatisfiedBy(PilotMember p) =>
            p.VehicleRestriction == _aircraft;

        public string? ErrorMessage => null;
    }
    public sealed class PilotRangeSpec
    : ISpecification<PilotMember>
    {
        private readonly double _distanceKm;
        public PilotRangeSpec(double distanceKm) => _distanceKm = distanceKm;

        public bool IsSatisfiedBy(PilotMember p) =>
            p.AllowedRangeKm >= _distanceKm;

        public string? ErrorMessage => null;
    }
}
