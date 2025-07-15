using MainSystem.Domain.Enums;
using MainSystem.Domain.Services.Factories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Domain.Entities
{
    [NotMapped]
    public class VehicleType
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public AircraftType AircraftType { get; private set; }
        public int SeatCount { get; private set; }
        public int MaxPassengers { get; private set; }
        public int MaxCrew { get; private set; }
        public string StandardMenu { get; private set; }

        private VehicleType() { }

        public VehicleType(AircraftType type,
                           int seatCount,
                           int maxPassengers,
                           int maxCrew,
                           string standardMenu)
        {
            AircraftType = type;
            SeatCount = seatCount;
            MaxPassengers = maxPassengers;
            MaxCrew = maxCrew;
            StandardMenu = standardMenu;
        }

        public ISeatPlan CreateSeatPlan() => SeatPlanFactory.Create(AircraftType);
    }
}
