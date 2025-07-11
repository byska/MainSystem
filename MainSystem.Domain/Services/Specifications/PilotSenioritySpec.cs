using MainSystem.Domain.Entities;
using MainSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Domain.Services.Specifications
{
    public sealed class PilotSenioritySpec : ISpecification<IEnumerable<PilotMember>>
    {
        public bool IsSatisfiedBy(IEnumerable<PilotMember> pilots) =>
        pilots.Any(p => p.Seniority == PilotSeniorityLevel.Senior) &&
        pilots.Any(p => p.Seniority == PilotSeniorityLevel.Junior);

        public string? ErrorMessage =>
            "En az bir Senior ve bir Junior pilot olmalıdır.";
    }
}
