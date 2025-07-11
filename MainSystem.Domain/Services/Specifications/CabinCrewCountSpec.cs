using MainSystem.Domain.Entities;
using MainSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Domain.Services.Specifications
{
    public sealed class CabinCrewCountSpec : ISpecification<IEnumerable<CabinAttendantMember>>
    {
        public bool IsSatisfiedBy(IEnumerable<CabinAttendantMember> crew)
        {
            int chiefs = crew.Count(c => c.Type == AttendantType.Chief);
            int juniors = crew.Count(c => c.Type == AttendantType.Regular);
            int chefs = crew.Count(c => c.Type == AttendantType.Chef);

            return chiefs is >= 1 and <= 4 &&
                   juniors is >= 4 and <= 16 &&
                   chefs is >= 0 and <= 2;
        }

        public string? ErrorMessage => "Kabin ekibi sayıları kuralı sağlanmıyor.";
    }
}
