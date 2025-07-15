using MainSystem.Domain.Entities;
using MainSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Domain.Services.Specifications
{
    public sealed class FlightByNumberSpec : ExpressionSpec.ExpressionSpecification<Flight>
    {
        private readonly FlightNumber _number;
        public FlightByNumberSpec(FlightNumber number) => _number = number;
        public override Expression<Func<Flight, bool>> ToExpression() =>
           fr => fr.FlightNo == _number;
    }
}
