using MainSystem.Domain.Enums;
using MainSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Infrastructure.External.Adapters.Dto
{
    public sealed record PilotDto(
     Guid Id,
     PersonInfo Info,
     AircraftType VehicleRestriction,
     double AllowedRangeKm,
     PilotSeniorityLevel SeniorityLevel);
}
