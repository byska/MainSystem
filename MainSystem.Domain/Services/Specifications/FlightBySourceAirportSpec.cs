using MainSystem.Domain.Entities;
using MainSystem.Domain.Services.Specifications.ExpressionSpec;
using MainSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace MainSystem.Domain.Services.Specifications
{
    public sealed class FlightBySourceAirportSpec : ExpressionSpecification<Flight>
    {
        private readonly AirportCode _code;
        public FlightBySourceAirportSpec(string srcCode)
            => _code = new AirportCode(srcCode);

        public override Expression<Func<Flight, bool>> ToExpression() =>
            fr => fr.SourceAirport.Code == _code;
    }

    public sealed class FlightByDestAirportSpec : ExpressionSpecification<Flight>
    {
        private readonly AirportCode _code;
        public FlightByDestAirportSpec(string destCode)
            => _code = new AirportCode(destCode);

        public override Expression<Func<Flight, bool>> ToExpression() =>
            fr => fr.DestinationAirport.Code == _code;
    }

    public sealed class FlightSharedSpec : ExpressionSpecification<Flight>
    {
        private readonly bool _shared;
        public FlightSharedSpec(bool isShared) => _shared = isShared;

        public override Expression<Func<Flight, bool>> ToExpression() =>
            fr => fr.IsShared == _shared;  
    }
}
