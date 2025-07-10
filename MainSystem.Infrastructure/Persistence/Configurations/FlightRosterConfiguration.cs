using MainSystem.Domain.Entities;
using MainSystem.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Infrastructure.Persistence.Configurations
{
    public sealed class FlightRosterConfiguration : IEntityTypeConfiguration<FlightRoster>
    {
        public void Configure(EntityTypeBuilder<FlightRoster> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.FlightNo)
                   .HasColumnName("FlightNumber")
                   .HasConversion(v => v.Value,
                                  v => new FlightNumber(v))
                   .HasMaxLength(6)
                   .IsRequired();

            builder.Property(x => x.DepartureTime)
                   .IsRequired();

            builder.Property(x => x.Aircraft)
                   .HasConversion<int>();         

            builder.HasMany<RosterMember>("_members")
                   .WithOne()
                   .HasForeignKey("FlightRosterId")
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Ignore(x => x.Pilots);
            builder.Ignore(x => x.CabinAttendants);
            builder.Ignore(x => x.Passengers);

            builder.HasIndex(x => x.FlightNo).IsUnique();
        }
    }
}
