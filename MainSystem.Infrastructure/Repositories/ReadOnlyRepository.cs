using MainSystem.Application.Abstraction;
using MainSystem.Domain.Services.Specifications;
using MainSystem.Domain.Services.Specifications.ExpressionSpec;
using MainSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Infrastructure.Repositories
{
    public class ReadOnlyRepository<T> : IReadOnlyRepository<T>
    where T : class
    {
        private readonly MainSystemDbContext _db;
        public ReadOnlyRepository(MainSystemDbContext db) => _db = db;
        public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _db.Set<T>().FindAsync(new object[] { id }, ct).AsTask();
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T>? spec = null, CancellationToken ct = default)
        {
            IQueryable<T> query = _db.Set<T>();

            if (spec is IExpressionSpecification<T> exprSpec)
            {
                Expression<Func<T, bool>> expr = exprSpec.ToExpression();
                query = query.Where(expr);
                return await query.ToListAsync(ct);
            }

            var list = await query.ToListAsync(ct);
            return spec is null
                ? list
                : list.Where(spec.IsSatisfiedBy).ToList();
        }
    }
}
