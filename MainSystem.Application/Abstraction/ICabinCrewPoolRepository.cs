using MainSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Application.Abstraction
{
    public interface ICabinCrewPoolRepository : IReadOnlyRepository<CabinAttendantMember>
    {
    }
}
