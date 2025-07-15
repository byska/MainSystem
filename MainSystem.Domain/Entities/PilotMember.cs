using MainSystem.Domain.Enums;
using MainSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Domain.Entities
{
    public class PilotMember : RosterMember
    {
        public PilotSeniorityLevel Seniority { get; private set; }
        public AircraftType VehicleRestriction { get; private set; }
        public double AllowedRangeKm { get; private set; }

        private PilotMember() { }

        public PilotMember(Guid personId,
                           PersonInfo info,
                           PilotSeniorityLevel seniority,
                           AircraftType vehicleRestriction,
                           double allowedRangeKm)
            : base(personId, info)
        {
            Seniority = seniority;
            VehicleRestriction = vehicleRestriction;
            AllowedRangeKm = allowedRangeKm;
        }
    }
}
