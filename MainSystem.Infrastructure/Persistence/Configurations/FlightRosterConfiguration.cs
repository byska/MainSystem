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
        public void Configure(EntityTypeBuilder<FlightRoster> b)
        {
            b.ToTable("FlightRosters");
            b.HasKey(r => r.Id);

            b.Property(r => r.CreatedAt)
             .IsRequired();

            // Flight sadece ID olarak saklanıyor
            b.Property<Guid>("FlightId")
             .IsRequired();

            // Flight navigation runtime’da doldurulacak, DB ile ilişkisi yok
            b.Ignore(r => r.Flight);

       
          

            // 🧑‍✈️ Pilots
            b.OwnsMany(r => r.Pilots, p =>
            {
                p.ToTable("FlightRoster_Pilots");
                p.WithOwner().HasForeignKey("RosterId");

                p.Property<Guid>("Id");
                p.HasKey("Id");

           
                
            });

            b.OwnsMany(r => r.Attendants, p =>
            {
                p.ToTable("FlightRoster_Attendants");
                p.WithOwner().HasForeignKey("RosterId");

                p.Property<Guid>("Id");
                p.HasKey("Id");

                p.Property(x => x.PersonId).IsRequired();

              
            });
            b.OwnsMany(r => r.Passengers, p =>
            {
                p.ToTable("FlightRoster_Passengers");
                p.WithOwner().HasForeignKey("RosterId");

                p.Property<Guid>("Id");
                p.HasKey("Id");


             
            });

        }
    }
}
