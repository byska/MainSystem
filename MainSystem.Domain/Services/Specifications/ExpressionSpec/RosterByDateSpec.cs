using MainSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Domain.Services.Specifications.ExpressionSpec
{
    public sealed class RosterByDateSpec: IExpressionSpecification<Flight>
    {
        private readonly DateOnly _date;
        public RosterByDateSpec(DateOnly date) => _date = date;

        public Expression<Func<Flight, bool>> ToExpression() =>
            r => DateOnly.FromDateTime(r.DepartureTime) == _date;

        public bool IsSatisfiedBy(Flight r) =>
            DateOnly.FromDateTime(r.DepartureTime) == _date;

        public string? ErrorMessage => null;
    }
}
