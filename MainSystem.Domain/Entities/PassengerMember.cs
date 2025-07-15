using MainSystem.Domain.Enums;
using MainSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Domain.Entities
{
    public class PassengerMember : RosterMember
    {
        public FlightNumber FlightNumber { get; private set; }
        public SeatNumber? SeatNumber { get; private set; }
        public SeatClass? SeatClass { get; private set; }
        public bool IsInfant { get; private set; }
        public Guid? ParentPassengerId { get; private set; }

        private readonly List<Guid> _affiliateIds = new();
        public IReadOnlyList<Guid> AffiliateIds => _affiliateIds;

        private PassengerMember() { }

        public PassengerMember(
         Guid id,
         FlightNumber flightNumber,
         PersonInfo info,
         bool isInfant,
         SeatClass? seatClass = null,
         SeatNumber? seatNumber = null,
         Guid? parentPassengerId = null,
         IEnumerable<Guid>? affiliatedIds = null)
         : base(id, info)
        {
            FlightNumber = flightNumber;
            SeatNumber = seatNumber;
            SeatClass = seatClass;
            IsInfant = isInfant;
            SeatNumber = seatNumber;
            ParentPassengerId = parentPassengerId;
        }

        public void AssignSeat(SeatNumber seat)
        {
            if (IsInfant)
                throw new InvalidOperationException("İnfant (0-2 yaş) yolculara koltuk atanmaz.");
            if (SeatNumber is not null)
                throw new InvalidOperationException("Yolcunun koltuğu zaten atanmış.");
            SeatNumber = seat;
        }
        public void AddAffiliate(Guid passengerId)
        {
            if (_affiliateIds.Count == 2)
                throw new InvalidOperationException("En fazla 2 bağlantılı yolcu olabilir.");
            _affiliateIds.Add(passengerId);
        }

    }
}
