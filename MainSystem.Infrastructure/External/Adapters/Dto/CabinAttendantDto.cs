using MainSystem.Domain.Enums;
using MainSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Infrastructure.External.Adapters.Dto
{
    public sealed record CabinAttendantDto(
    Guid Id,
    PersonInfo Info,             
    AttendantType AttendantType,    
    List<string> VehicleRestrictions,
    List<string>? Recipes);
}
