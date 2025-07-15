using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Domain.ValueObjects
{
    public sealed record SeatNumber
    {
        public int Row { get; private set; }
        public char Column { get; private set; }
        public SeatNumber()
        {
            
        }
        public SeatNumber(int row, char column)
        {
            if (row <= 0) throw new ArgumentException("Row must be positive.", nameof(row));
            if (!char.IsLetter(column)) throw new ArgumentException("Column must be A-Z.", nameof(column));

            Row = row;
            Column = char.ToUpperInvariant(column);
        }

        public override string ToString() => $"{Row}{Column}";

        public static SeatNumber Parse(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Null/empty seat.", nameof(value));

            var digits = new string(value.TakeWhile(char.IsDigit).ToArray());
            var letter = value.SkipWhile(char.IsDigit).FirstOrDefault();

            if (!int.TryParse(digits, out var row) || !char.IsLetter(letter))
                throw new FormatException($"Geçersiz koltuk formatı: {value}");

            return new SeatNumber(row, char.ToUpperInvariant(letter));
        }

        public static bool TryParse(string? value, out SeatNumber? seat)
        {
            seat = null;
            try { seat = Parse(value!); return true; }
            catch { return false; }
        }
    }
}
