using MainSystem.Domain.Enums;
using MainSystem.Domain.Services.Factories;
using MainSystem.Domain.Services.Strategies;
using MainSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Domain.Entities
{
    public class FlightRoster
    {
        public Guid Id { get; private set; }
        public Guid FlightId { get; private set; }        
        [NotMapped]
        public Flight Flight { get; private set; }

        private readonly List<PilotMember> _pilots = [];
        private readonly List<CabinAttendantMember> _attendants = [];
        private readonly List<PassengerMember> _passengers = [];

        public IReadOnlyList<PilotMember> Pilots => _pilots;
        public IReadOnlyList<CabinAttendantMember> Attendants => _attendants;
        public IReadOnlyList<PassengerMember> Passengers => _passengers;

        public DateTime CreatedAt { get; private set; }

        private FlightRoster() { }

        public FlightRoster(Flight flight)
        {
            Id = Guid.NewGuid();
            Flight = flight ?? throw new ArgumentNullException(nameof(flight));     
            CreatedAt = DateTime.UtcNow;
        }

        public void AddPilot(PilotMember pilot)
        {
            if (pilot is null) throw new ArgumentNullException(nameof(pilot));
            if (pilot.VehicleRestriction != Flight.Aircraft)
                throw new InvalidOperationException("Pilot bu uçak tipini kullanamaz.");
            if (Flight.DistanceKm > pilot.AllowedRangeKm)
                throw new InvalidOperationException("Mesafe, pilotun izin verilen menzilini aşıyor.");

            ValidatePilotComposition();
            _pilots.Add(pilot);
        }

        public void RemovePilot(Guid pilotId)      
        {
            ValidatePilotComposition();
            _pilots.RemoveAll(p => p.Id == pilotId);
        }

        private void ValidatePilotComposition()
        {
            int senior = _pilots.Count(p => p.Seniority == PilotSeniorityLevel.Senior);
            int junior = _pilots.Count(p => p.Seniority == PilotSeniorityLevel.Junior);
            int trainee = _pilots.Count(p => p.Seniority == PilotSeniorityLevel.Trainee);

            if (senior < 1 || junior < 1)
                throw new InvalidOperationException("Her uçuşta en az 1 senior ve 1 junior pilot olmalı.");
            if (trainee > 2)
                throw new InvalidOperationException("En fazla 2 trainee pilot olabilir.");
            if (_pilots.Count > Flight.MaxCrew)
                throw new InvalidOperationException("Pilot sayısı uçak limitini aşıyor.");
        }

        public void AddAttendant(CabinAttendantMember ca)
        {
            if (ca is null) throw new ArgumentNullException(nameof(ca));
            if (!ca.VehicleRestrictions.Contains(Flight.Aircraft))
                throw new InvalidOperationException("Kabin görevlisi bu uçak tipine atanamaz.");

            ValidateAttendantComposition();
            _attendants.Add(ca);
        }

        private void ValidateAttendantComposition()
        {
            int senior = _attendants.Count(a => a.Type == AttendantType.Chief);
            int junior = _attendants.Count(a => a.Type == AttendantType.Regular);
            int chefs = _attendants.Count(a => a.Type == AttendantType.Chef);

            if (senior is < 1 or > 4)
                throw new InvalidOperationException("Chief (kıdemli) sayısı 1-4 arası olmalı.");
            if (junior is < 4 or > 16)
                throw new InvalidOperationException("Junior sayısı 4-16 arası olmalı.");
            if (chefs is > 2)
                throw new InvalidOperationException("En fazla 2 şef (chef) olabilir.");
            if (_attendants.Count > Flight.MaxCrew)
                throw new InvalidOperationException("Toplam kabin ekibi, uçak limitini aşıyor.");
        }

        public void AddPassenger(PassengerMember pax)
        {
            if (pax is null) throw new ArgumentNullException(nameof(pax));
            if (_passengers.Count >= Flight.MaxPassengers)
                throw new InvalidOperationException("Yolcu kapasitesi dolu.");
            if (pax.FlightNumber != Flight.FlightNo)
                throw new InvalidOperationException("Yolcunun uçuş numarası eşleşmiyor.");

            _passengers.Add(pax);
        }

        public void AssignSeats(ISeatAssignmentStrategy? strategy = null)
        {
            strategy ??= new GreedySeatAssignmentStrategy();
            strategy.AssignSeats(this);
        }
    }
}
