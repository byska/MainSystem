using MainSystem.Domain.Entities;
using MainSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Domain.Services.Strategies
{
    public sealed class RandomSeatAssignmentStrategy : ISeatAssignmentStrategy
    {
        public string Key => "Random";

        public void AssignSeats(FlightRoster roster)
        {
            foreach (SeatClass seatClass in Enum.GetValues(typeof(SeatClass)))
            {
                var passengers = roster.Passengers
                                       .Where(p => p.SeatClass == seatClass && p.SeatNumber is null)
                                       .ToList();
                if (passengers.Count == 0) continue;

                var seats = SeatAssignmentHelpers.GetAvailableSeats(roster, seatClass);
                SeatAssignmentHelpers.Shuffle(seats);

                for (int i = 0; i < passengers.Count && i < seats.Count; i++)
                    passengers[i].AssignSeat(seats[i]);
            }
        }
    }
}
