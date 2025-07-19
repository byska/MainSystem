using MainSystem.Domain.Entities;
using MainSystem.Domain.Enums;
using MainSystem.Domain.ValueObjects;

namespace MainSystem.Tests.Helpers.TestData;

public static class TestFlightFactory
{
    public static Flight CreateTestFlight(AircraftType aircraft = AircraftType.Boeing737)
    {
        var source = new Airport("Turkey", "Istanbul", "Istanbul Airport", new AirportCode("IST"));
        var destination = new Airport("UK", "London", "Heathrow", new AirportCode("LHR"));
        var vehicle = CreateVehicleType(aircraft);

        return new Flight(
            new FlightNumber("TK1234"),
            DateTime.UtcNow.AddHours(2),
            new FlightDuration(TimeSpan.FromHours(4)),
            2500,
            source,
            destination,
            RouteType.International,
            vehicle
        );
    }

    public static Flight CreateSmallFlight(int maxPassengers = 50)
    {
        var source = new Airport("Turkey", "Istanbul", "Istanbul Airport", new AirportCode("IST"));
        var destination = new Airport("Turkey", "Ankara", "Ankara Airport", new AirportCode("ESB"));
        var vehicle = new VehicleType(AircraftType.Embraer190, 100, maxPassengers, 20, "Standard Menu");

        return new Flight(
            new FlightNumber("TK5678"),
            DateTime.UtcNow.AddHours(1),
            new FlightDuration(TimeSpan.FromHours(1)),
            350,
            source,
            destination,
            RouteType.Domestic,
            vehicle
        );
    }

    private static VehicleType CreateVehicleType(AircraftType aircraft)
    {
        return aircraft switch
        {
            AircraftType.Boeing737 => new VehicleType(aircraft, 189, 180, 20, "Standard"),
            AircraftType.AirbusA320 => new VehicleType(aircraft, 180, 170, 18, "Standard"),
            _ => new VehicleType(aircraft, 150, 140, 15, "Standard")
        };
    }
}

public static class TestPilotFactory
{
    public static PilotMember CreateSeniorPilot(AircraftType aircraft = AircraftType.Boeing737)
    {
        return CreatePilot(PilotSeniorityLevel.Senior, aircraft);
    }

    public static PilotMember CreateJuniorPilot(AircraftType aircraft = AircraftType.Boeing737)
    {
        return CreatePilot(PilotSeniorityLevel.Junior, aircraft);
    }

    public static PilotMember CreatePilot(
        PilotSeniorityLevel seniority = PilotSeniorityLevel.Senior,
        AircraftType aircraftType = AircraftType.Boeing737)
    {
        var info = new PersonInfo(
            $"Pilot {seniority}",
            35,
            "Male",
            "Turkish",
            new[] { "Turkish", "English" }
        );

        return new PilotMember(Guid.NewGuid(), info, seniority, aircraftType, 15000);
    }

    public static IReadOnlyList<PilotMember> CreateValidPilotCrew()
    {
        return new List<PilotMember>
        {
            CreateSeniorPilot(),
            CreateJuniorPilot(),
            CreatePilot(PilotSeniorityLevel.Trainee)
        };
    }
}

public static class TestAttendantFactory
{
    public static CabinAttendantMember CreateChiefAttendant()
    {
        var info = new PersonInfo("Chief Attendant", 30, "Female", "Turkish", new[] { "Turkish", "English" });
        return new CabinAttendantMember(
            Guid.NewGuid(),
            info,
            AttendantType.Chief,
            new[] { AircraftType.Boeing737, AircraftType.AirbusA320 }
        );
    }

    public static CabinAttendantMember CreateRegularAttendant()
    {
        var info = new PersonInfo("Flight Attendant", 25, "Female", "Turkish", new[] { "Turkish", "English" });
        return new CabinAttendantMember(
            Guid.NewGuid(),
            info,
            AttendantType.Regular,
            new[] { AircraftType.Boeing737, AircraftType.AirbusA320 }
        );
    }

    public static CabinAttendantMember CreateChef()
    {
        var info = new PersonInfo("Chef", 35, "Male", "French", new[] { "French", "English" });
        return new CabinAttendantMember(
            Guid.NewGuid(),
            info,
            AttendantType.Chef,
            new[] { AircraftType.Boeing737 },
            new[] { "Coq au Vin", "Beef Bourguignon" }
        );
    }

    public static IReadOnlyList<CabinAttendantMember> CreateValidCabinCrew()
    {
        var crew = new List<CabinAttendantMember>
        {
            CreateChiefAttendant(),
            CreateChef()
        };

        // Add 4 regular attendants (minimum required)
        for (int i = 0; i < 4; i++)
        {
            crew.Add(CreateRegularAttendant());
        }

        return crew;
    }
}

public static class TestPassengerFactory
{
    public static PassengerMember CreatePassenger(string flightNumber = "TK1234")
    {
        var info = new PersonInfo("John Doe", 30, "Male", "American", new[] { "English" });
        return new PassengerMember(
            Guid.NewGuid(),
            new FlightNumber(flightNumber),
            info,
            false,
            SeatClass.Economy
        );
    }

    public static PassengerMember CreateBusinessPassenger(string flightNumber = "TK1234")
    {
        var info = new PersonInfo("Jane Smith", 40, "Female", "British", new[] { "English" });
        return new PassengerMember(
            Guid.NewGuid(),
            new FlightNumber(flightNumber),
            info,
            false,
            SeatClass.Business
        );
    }

    public static PassengerMember CreateInfant(string flightNumber = "TK1234", Guid? parentId = null)
    {
        var info = new PersonInfo("Baby Doe", 1, "Male", "American", new[] { "English" });
        return new PassengerMember(
            Guid.NewGuid(),
            new FlightNumber(flightNumber),
            info,
            true,
            null,
            null,
            parentId
        );
    }

    public static IReadOnlyList<PassengerMember> CreatePassengerList(int count, string flightNumber = "TK1234")
    {
        var passengers = new List<PassengerMember>();
        
        for (int i = 0; i < count; i++)
        {
            var info = new PersonInfo($"Passenger {i}", 20 + i, "Male", "Turkish", new[] { "Turkish" });
            passengers.Add(new PassengerMember(
                Guid.NewGuid(),
                new FlightNumber(flightNumber),
                info,
                false,
                i < count / 5 ? SeatClass.Business : SeatClass.Economy
            ));
        }

        return passengers;
    }
}