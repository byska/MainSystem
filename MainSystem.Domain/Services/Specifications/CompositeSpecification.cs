using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Domain.Services.Specifications
{
    public abstract class CompositeSpecification<T> : ISpecification<T>
    {
        public abstract bool IsSatisfiedBy(T candidate);
        public virtual string? ErrorMessage => null;

        public ISpecification<T> And(ISpecification<T> other) =>
            new AndSpecification<T>(this, other);

        public ISpecification<T> Or(ISpecification<T> other) =>
            new OrSpecification<T>(this, other);

        public ISpecification<T> Not() => new NotSpecification<T>(this);
        private sealed class AndSpecification<TC> : CompositeSpecification<TC>
        {
            private readonly ISpecification<TC> _left, _right;
            private string? _error;

            public AndSpecification(ISpecification<TC> left, ISpecification<TC> right)
            {
                _left = left;
                _right = right;
            }

            public override bool IsSatisfiedBy(TC candidate)
            {
                if (!_left.IsSatisfiedBy(candidate))
                {
                    _error = _left.ErrorMessage;
                    return false;
                }

                if (!_right.IsSatisfiedBy(candidate))
                {
                    _error = _right.ErrorMessage;
                    return false;
                }

                _error = null;
                return true;
            }

            public override string? ErrorMessage => _error;
        }

        private sealed class OrSpecification<TC> : CompositeSpecification<TC>
        {
            private readonly ISpecification<TC> _left, _right;
            private string? _error;

            public OrSpecification(ISpecification<TC> left, ISpecification<TC> right)
            {
                _left = left;
                _right = right;
            }

            public override bool IsSatisfiedBy(TC candidate)
            {
                if (_left.IsSatisfiedBy(candidate) || _right.IsSatisfiedBy(candidate))
                {
                    _error = null;
                    return true;
                }

                // İkisi de başarısızsa soldaki mesajı, yoksa sağdaki mesajı döndür.
                _error = _left.ErrorMessage ?? _right.ErrorMessage;
                return false;
            }

            public override string? ErrorMessage => _error;
        }
        private sealed class NotSpecification<TC> : CompositeSpecification<TC>
        {
            private readonly ISpecification<TC> _inner;
            private string? _error;

            public NotSpecification(ISpecification<TC> inner)
            {
                _inner = inner;
            }

            public override bool IsSatisfiedBy(TC candidate)
            {
                if (_inner.IsSatisfiedBy(candidate))
                {
                    _error = $"Negated spec “{_inner.GetType().Name}” geçtiği için başarısız.";
                    return false;
                }

                _error = null;
                return true;
            }

            public override string? ErrorMessage => _error;
        }
    }
}
