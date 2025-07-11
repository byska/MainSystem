using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Domain.ValueObjects
{
    public class FlightDuration
    {
        public TimeSpan Value { get; }

        public FlightDuration(TimeSpan value)
        {
            if (value.TotalMinutes < 5)
                throw new ArgumentException("Flight duration must be at least 5 minutes.");

            Value = value;
        }

        public override bool Equals(object? obj) =>
            obj is FlightDuration other && Value.Equals(other.Value);

        public override int GetHashCode() => Value.GetHashCode();

        public static implicit operator TimeSpan(FlightDuration duration) => duration.Value;
        public override string ToString() => Value.ToString();
    }
}
