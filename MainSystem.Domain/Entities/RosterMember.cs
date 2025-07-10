using MainSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Domain.Entities
{
    public abstract class RosterMember
    {
        public Guid Id { get; protected set; }  
        public Guid PersonId { get; protected set; }  
        public PersonInfo Info { get; protected set; }

        protected RosterMember() { }  

        protected RosterMember(Guid personId, PersonInfo info)
        {
            Id = Guid.NewGuid();
            PersonId = personId;
            Info = info ?? throw new ArgumentNullException(nameof(info));
        }
    }
}
