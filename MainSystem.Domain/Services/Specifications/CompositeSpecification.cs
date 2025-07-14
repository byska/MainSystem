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

        public CompositeSpecification<T> And(ISpecification<T> other) =>
            new AndSpecification(this, other);

        public CompositeSpecification<T> Or(ISpecification<T> other) =>
            new OrSpecification(this, other);

        public CompositeSpecification<T> Not() => new NotSpecification(this);
        private sealed class AndSpecification : CompositeSpecification<T>
        {
            private readonly ISpecification<T> _left, _right;
            private string? _error;

            public AndSpecification(ISpecification<T> left, ISpecification<T> right)
            {
                _left = left;
                _right = right;
            }

            public override bool IsSatisfiedBy(T candidate)
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

        private sealed class OrSpecification : CompositeSpecification<T>
        {
            private readonly ISpecification<T> _left, _right;
            private string? _error;

            public OrSpecification(ISpecification<T> left, ISpecification<T> right)
            {
                _left = left;
                _right = right;
            }

            public override bool IsSatisfiedBy(T candidate)
            {
                if (_left.IsSatisfiedBy(candidate) || _right.IsSatisfiedBy(candidate))
                {
                    _error = null;
                    return true;
                }

                _error = _left.ErrorMessage ?? _right.ErrorMessage;
                return false;
            }

            public override string? ErrorMessage => _error;
        }
        private sealed class NotSpecification : CompositeSpecification<T>
        {
            private readonly ISpecification<T> _inner;
            private string? _error;

            public NotSpecification(ISpecification<T> inner)
            {
                _inner = inner;
            }

            public override bool IsSatisfiedBy(T candidate)
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
