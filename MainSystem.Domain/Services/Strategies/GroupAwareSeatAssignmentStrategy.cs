using MainSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Domain.Services.Strategies
{
    public sealed class GroupAwareSeatAssignmentStrategy : ISeatAssignmentStrategy
    {
        public string Key => "Group";

        public void AssignSeats(FlightRoster roster)
        {
            // 1) Grupları kur (ebeveyn–çocuk ve affiliated zincirlerini tek kümeye toplar)
            var groups = BuildGroups(roster.Passengers.Where(p => p.SeatNumber is null));

            // 2) Her grup için blok ara
            foreach (var grp in groups)
            {
                var seatClass = grp[0].SeatClass;                    
                var freeSeats = SeatAssignmentHelpers.GetAvailableSeats(roster, seatClass);
                var block = SeatAssignmentHelpers.FindAdjacentBlock(freeSeats, grp.Count);

                if (block is not null)
                {
                    for (int i = 0; i < grp.Count; i++)
                        grp[i].AssignSeat(block[i]);
                }
            }

            // 3) Hâlâ koltuğu olmayanlar → Greedy
            var greedy = new GreedySeatAssignmentStrategy();
            greedy.AssignSeats(roster);
        }

        /// Yolcuları parent/affiliated bağına göre kümeler.
        private static List<List<PassengerMember>> BuildGroups(IEnumerable<PassengerMember> source)
        {
            var dict = source.ToDictionary(p => p.Id);
            var visited = new HashSet<Guid>();
            var result = new List<List<PassengerMember>>();

            foreach (var p in source)
            {
                if (visited.Contains(p.Id)) continue;
                var list = new List<PassengerMember>();
                DFS(p, list);
                result.Add(list);
            }
            return result;

            void DFS(PassengerMember pm, List<PassengerMember> acc)
            {
                if (!visited.Add(pm.Id)) return;
                acc.Add(pm);

                if (pm.ParentPassengerId is Guid parentId && dict.TryGetValue(parentId, out var parent))
                    DFS(parent, acc);

                foreach (var childId in pm.AffiliatedPassengerIds)
                    if (dict.TryGetValue(childId, out var child))
                        DFS(child, acc);
            }
        }
    }
}
