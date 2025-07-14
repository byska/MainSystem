using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Domain.Services.Specifications
{
    public sealed class TrueSpec<T> : CompositeSpecification<T>
    {

        public override bool IsSatisfiedBy(T candidate)
        {
            return true;
        }

        public string? ErrorMessage => null;
    }

    public sealed class FalseSpec<T> : CompositeSpecification<T>
    {
        public override bool IsSatisfiedBy(T candidate)
        {
            return false;
        }
        public override string? ErrorMessage => "Koşul her zaman başarısız.";
    }
}
