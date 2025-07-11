using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Domain.Services.Specifications.ExpressionSpec
{
    public interface IExpressionSpecification<T> : ISpecification<T>
    {
        Expression<Func<T, bool>> ToExpression();
    }
}
