using MainSystem.Domain.Entities;
using MainSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Domain.Services.Specifications
{
    public class CabinCrewVehicleSpec : ISpecification<CabinAttendantMember>
    {
        private readonly AircraftType _aircraft;
        public CabinCrewVehicleSpec(AircraftType aircraft) => _aircraft = aircraft;

        public bool IsSatisfiedBy(CabinAttendantMember c) =>
            c.VehicleRestrictions.Contains(_aircraft);

        public string? ErrorMessage => null;
    }
}
