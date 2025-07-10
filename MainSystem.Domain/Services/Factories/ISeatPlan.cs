using MainSystem.Domain.Enums;
using MainSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Domain.Services.Factories
{
    public interface ISeatPlan
    {
        int TotalSeats { get; }
        IEnumerable<SeatNumber> AllSeats();
        IEnumerable<SeatNumber> GetSeats(SeatClass seat);
        SeatClass GetClass(SeatNumber seat);
    }
}
