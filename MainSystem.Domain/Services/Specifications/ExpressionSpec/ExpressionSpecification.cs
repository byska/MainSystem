using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Domain.Services.Specifications.ExpressionSpec
{
    public abstract class ExpressionSpecification<T> : CompositeSpecification<T>, IExpressionSpecification<T>
    {
        public abstract Expression<Func<T, bool>> ToExpression();

        public override bool IsSatisfiedBy(T candidate) =>
            ToExpression().Compile()(candidate);
    }
}
