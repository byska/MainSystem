using MainSystem.Domain.Entities;
using MainSystem.Domain.Enums;
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
    public sealed class RosterMemberConfiguration : IEntityTypeConfiguration<RosterMember>
    {
        public void Configure(EntityTypeBuilder<RosterMember> builder)
        {
            builder.ToTable("RosterMembers");

            builder.HasKey(x => x.Id);

            builder.UseTphMappingStrategy()
                   .HasDiscriminator<string>("MemberType")
                   .HasValue<PilotMember>("Pilot")
                   .HasValue<CabinAttendantMember>("CabinAttendant")
                   .HasValue<PassengerMember>("Passenger");

            builder.Property<Guid>("FlightRosterId");   

            builder.Property(x => x.PersonId)
                   .IsRequired();

            builder.OwnsOne(x => x.Info, pi =>
            {
                pi.Property(p => p.Name).HasColumnName("Name").HasMaxLength(100);
                pi.Property(p => p.Age);
                pi.Property(p => p.Gender).HasMaxLength(20);
                pi.Property(p => p.Nationality).HasMaxLength(50);

                pi.Property(p => p.Languages)
                  .HasColumnName("Languages")
                  .HasConversion(
                      v => string.Join(';', v),
                      v => v.Split(';', StringSplitOptions.RemoveEmptyEntries))
                  .HasMaxLength(400);
            });

            builder.Property<PilotSeniorityLevel?>("Seniority")
                   .HasColumnName("PilotSeniority")
                   .HasConversion<int>();

            builder.Property<AircraftType?>("VehicleRestriction")
                   .HasColumnName("PilotAircraftRestriction")
                   .HasConversion<int>();

            builder.Property<double?>("AllowedRangeKm")
                   .HasColumnName("PilotAllowedRangeKm");

            builder.Property<AttendantType?>("AttendantType")
                   .HasColumnName("AttendantType")
                   .HasConversion<int>();

            builder.Property<string?>("AttendantVehicleRestrictions")
                   .HasConversion(
                        v => string.Join(',', v!),
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                              .Select(s => Enum.Parse<AircraftType>(s))
                              .ToList());

            builder.Property<string?>("DishRecipes") 
                   .HasConversion(
                        v => string.Join('|', v!),
                        v => v.Split('|', StringSplitOptions.RemoveEmptyEntries)
                              .ToList());

            builder.Property<SeatClass?>("SeatClass")
                   .HasConversion<int>();

            builder.OwnsOne<SeatNumber>("SeatNumber", sn =>
            {
                sn.Property(p => p.Row).HasColumnName("SeatRow");
                sn.Property(p => p.Column).HasColumnName("SeatCol")
                                          .HasMaxLength(1);
            });

            builder.Property<bool?>("IsInfant");
            builder.Property<Guid?>("ParentPassengerId");

            builder.Property<string?>("AffiliatedPassengerIds")
                   .HasConversion(
                       v => string.Join(',', v!),
                       v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                             .Select(Guid.Parse)
                             .ToList());
        }
    }
}
