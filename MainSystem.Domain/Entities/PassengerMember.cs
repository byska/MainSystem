using MainSystem.Domain.Enums;
using MainSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Domain.Entities
{
    public class PassengerMember : RosterMember
    {
        public SeatClass SeatClass { get; private set; }
        public SeatNumber? SeatNumber { get; private set; }
        public bool IsInfant { get; private set; }
        public Guid? ParentPassengerId { get; private set; }
        public List<Guid> AffiliatedPassengerIds { get; private set; } = [];

        private PassengerMember() { }

        public PassengerMember(Guid personId,
                               PersonInfo info,
                               SeatClass seatClass,
                               bool isInfant,
                               SeatNumber? seatNumber = null,
                               Guid? parentPassengerId = null,
                               IEnumerable<Guid>? affiliatedIds = null)
            : base(personId, info)
        {
            SeatClass = seatClass;
            IsInfant = isInfant;
            SeatNumber = seatNumber;
            ParentPassengerId = parentPassengerId;

            if (affiliatedIds is not null)
                AffiliatedPassengerIds = affiliatedIds.ToList();
        }

        public void AssignSeat(SeatNumber seat) => SeatNumber = seat;
    }
}
