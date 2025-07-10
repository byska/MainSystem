using MainSystem.Domain.Enums;
using MainSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Domain.Entities
{
    public class CabinAttendantMember : RosterMember
    {
        public AttendantType Type { get; private set; }
        public List<string> DishRecipes { get; private set; } = [];
        public ICollection<AircraftType> VehicleRestrictions { get; private set; } = [];

        private CabinAttendantMember() { }

        public CabinAttendantMember(Guid personId,
                                    PersonInfo info,
                                    AttendantType type,
                                    IEnumerable<AircraftType> vehicleRestrictions,
                                    IEnumerable<string>? recipes = null)
            : base(personId, info)
        {
            Type = type;
            VehicleRestrictions = vehicleRestrictions?.ToList()
                                  ?? throw new ArgumentNullException(nameof(vehicleRestrictions));
            if (Type == AttendantType.Chef && recipes is not null)
                DishRecipes = recipes.ToList();
        }
    }
}
