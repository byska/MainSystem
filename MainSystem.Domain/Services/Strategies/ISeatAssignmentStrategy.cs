using MainSystem.Domain.Entities;
using MainSystem.Domain.Services.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Domain.Services.Strategies
{
    public interface ISeatAssignmentStrategy
    {
        string Key { get; }
    
        void AssignSeats(FlightRoster roster);
    }
}
