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
    public class Airport
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Country { get; private set; }
        public string City { get; private set; }
        public string Name { get; private set; }
        public AirportCode Code { get; private set; }          

        private Airport() { } 

        public Airport(string country, string city, string name, AirportCode code)
        {
            Country = country ?? throw new ArgumentNullException(nameof(country));
            City = city ?? throw new ArgumentNullException(nameof(city));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Code = code;
        }

        public override string ToString() => $"{Code} – {City}, {Country}";
    }
}
