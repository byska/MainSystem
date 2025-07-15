using MainSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Domain.Entities
{
    [NotMapped]
    public class SharedFlight
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public FlightNumber PartnerFlightNumber { get; private set; }   // "BB5678"
        public string PartnerCompany { get; private set; }

        public ConnectingFlight? Connecting { get; private set; }

        private SharedFlight() { }

        public SharedFlight(FlightNumber partnerFlightNumber, string partnerCompany)
        {
            PartnerFlightNumber = partnerFlightNumber;
            PartnerCompany = partnerCompany ?? throw new ArgumentNullException(nameof(partnerCompany));
        }
        public void AddConnectingFlight(string destination, DateTime departure)
        {
            if (Connecting is not null)
                throw new InvalidOperationException("Bağlantılı uçuş zaten mevcut.");

            Connecting = new ConnectingFlight(destination, departure);
        }

    }
    public class ConnectingFlight
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Destination { get; private set; }
        public DateTime Departure { get; private set; }

        private ConnectingFlight() { }

        internal ConnectingFlight(string destination, DateTime departure)
        {
            if (string.IsNullOrWhiteSpace(destination))
                throw new ArgumentException("Varış noktası gerekli.", nameof(destination));
            if (departure <= DateTime.UtcNow)
                throw new ArgumentException("Kalkış zamanı geçmiş olamaz.", nameof(departure));

            Destination = destination;
            Departure = departure;
        }
    }
}
