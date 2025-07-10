using MainSystem.Domain.Entities;
using MainSystem.Domain.Enums;
using MainSystem.Domain.Services.Factories;
using MainSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Domain.Services.Strategies
{
    internal static class SeatAssignmentHelpers
    {
        internal static List<SeatNumber> GetAvailableSeats(FlightRoster roster, SeatClass seatClass)
        {
            ISeatPlan plan = SeatPlanFactory.Create(roster.Aircraft);    
            return plan.GetSeats(seatClass)                           
                       .Except(roster.Passengers
                                     .Where(p => p.SeatNumber is not null)
                                     .Select(p => p.SeatNumber!))
                       .OrderBy(s => s.Row)
                       .ThenBy(s => s.Column)
                       .ToList();
        }


        internal static void Shuffle<T>(IList<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Shared.Next(i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        internal static IList<SeatNumber>? FindAdjacentBlock(IList<SeatNumber> list, int count)
        {
            var grouped = list.GroupBy(s => s.Row).OrderBy(g => g.Key);
            foreach (var row in grouped)
            {
                var ordered = row.OrderBy(s => s.Column).ToList();
                for (int i = 0; i <= ordered.Count - count; i++)
                {
                    bool contiguous = true;
                    for (int k = 1; k < count && contiguous; k++)
                        contiguous &= ordered[i + k].Column == ordered[i + k - 1].Column + 1;

                    if (contiguous)
                        return ordered.GetRange(i, count);
                }
            }
            return null;
        }
    }
}
