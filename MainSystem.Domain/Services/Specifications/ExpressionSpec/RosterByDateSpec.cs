using MainSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Domain.Services.Specifications.ExpressionSpec
{
    public sealed class RosterByDateSpec: IExpressionSpecification<FlightRoster>
    {
        private readonly DateOnly _date;
        public RosterByDateSpec(DateOnly date) => _date = date;

        public Expression<Func<FlightRoster, bool>> ToExpression() =>
            r => DateOnly.FromDateTime(r.DepartureTime) == _date;

        public bool IsSatisfiedBy(FlightRoster r) =>
            DateOnly.FromDateTime(r.DepartureTime) == _date;

        public string? ErrorMessage => null;
    }
}
