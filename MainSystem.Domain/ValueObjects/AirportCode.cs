using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MainSystem.Domain.ValueObjects
{
    public class AirportCode
    {
        public string Value { get; private set; }
        public AirportCode()
        {
            
        }
        public AirportCode(string value)
        {
            if (!Regex.IsMatch(value, "^[A-Z]{3}$"))
                throw new ArgumentException("Airport code must be 3 uppercase letters (e.g., IST, CDG).");

            Value = value;
        }

        public override bool Equals(object? obj) =>
            obj is AirportCode other && Value == other.Value;

        public override int GetHashCode() => Value.GetHashCode();

        public static implicit operator string(AirportCode code) => code.Value;
        public override string ToString() => Value;
    }
}
