using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Domain.ValueObjects
{
    public sealed record SeatNumber
    {
        public int Row { get; }
        public char Column { get; }

        public SeatNumber(int row, char column)
        {
            if (row <= 0) throw new ArgumentException("Row must be positive.", nameof(row));
            if (!char.IsLetter(column)) throw new ArgumentException("Column must be A-Z.", nameof(column));

            Row = row;
            Column = char.ToUpperInvariant(column);
        }

        public override string ToString() => $"{Row}{Column}";
    }
}
