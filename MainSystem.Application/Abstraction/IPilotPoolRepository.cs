using MainSystem.Domain.Entities;
using MainSystem.Domain.Services.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Application.Abstraction
{
    public interface IPilotPoolRepository : IReadOnlyRepository<PilotMember>
    {
       
    }
}
