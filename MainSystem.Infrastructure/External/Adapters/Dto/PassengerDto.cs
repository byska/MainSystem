using MainSystem.Domain.Enums;
using MainSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Infrastructure.External.Adapters.Dto
{
    public sealed record class PassengerDto(Guid Id,
    string FlightNumber, 
    SeatClass SeatClass,
    bool IsInfant,
    Guid ParentPassengerId,
    PersonInfo PassengerInfo,
    SeatNumber? SeatNumber, 
    List<Guid>? AffiliatedPassengerIds);

   
}
