using MainSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Infrastructure.Persistence
{
    public class MainSystemDbContext : DbContext
    {
        public DbSet<FlightRoster> Rosters => Set<FlightRoster>();
        public MainSystemDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MainSystemDbContext).Assembly);
        }
    }
}
