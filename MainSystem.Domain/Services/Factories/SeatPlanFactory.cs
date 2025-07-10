using MainSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Domain.Services.Factories
{
    public static class SeatPlanFactory
    {
        public static ISeatPlan Create(AircraftType type) => type switch
        {
            AircraftType.AirbusA320 => new AirbusA320Plan(),
            AircraftType.Boeing737 => new Boeing737Plan(),
            _ => throw new NotSupportedException($"Seat plan tanımlı değil: {type}")
        };
    }
}
