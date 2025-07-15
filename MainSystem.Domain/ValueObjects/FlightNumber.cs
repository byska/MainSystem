using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MainSystem.Domain.ValueObjects
{
    public sealed record FlightNumber
    {
        public string Value { get; private set; }
        public FlightNumber()
        {
            
        }
        public FlightNumber(string value)
        {
            if (!Regex.IsMatch(value, "^[A-Z]{2}\\d{4}$"))
                throw new ArgumentException("Flight number must be in AANNNN format.", nameof(value));
            Value = value;
        }
        public static implicit operator string(FlightNumber fn) => fn.Value;
    }
}
