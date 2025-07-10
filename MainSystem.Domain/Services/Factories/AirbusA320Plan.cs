using MainSystem.Domain.Enums;
using MainSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Domain.Services.Factories
{
    public sealed class AirbusA320Plan : ISeatPlan
    {
        private static readonly IReadOnlyList<SeatNumber> Seats =
        Enumerable.Range(1, 30)                 
                  .SelectMany(r => "ABCDEF"    
                                   .Select(c => new SeatNumber(r, c)))
                  .ToList();

        public int TotalSeats => Seats.Count;
        public IEnumerable<SeatNumber> AllSeats() => Seats;

        public SeatClass GetClass(SeatNumber seat) =>
            seat.Row <= 5 ? SeatClass.Business : SeatClass.Economy;

    }
}
