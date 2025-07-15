using MainSystem.Domain.Entities;
using MainSystem.Domain.ValueObjects;
using MainSystem.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Infrastructure.Persistence
{
    public class MainSystemDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<FlightRoster> Rosters => Set<FlightRoster>();
        public MainSystemDbContext(DbContextOptions<MainSystemDbContext> options)
         : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Ignore<AirportCode>();
            modelBuilder.Ignore<FlightDuration>();
            modelBuilder.Ignore<FlightNumber>();
            modelBuilder.Ignore<PersonInfo>();
            modelBuilder.Ignore<SeatNumber>();
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MainSystemDbContext).Assembly);
        }
    }
}
