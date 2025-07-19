using Xunit;
using FluentAssertions;
using MainSystem.Domain.Entities;
using MainSystem.Domain.Enums;
using MainSystem.Domain.ValueObjects;
using MainSystem.Tests.Helpers.TestData;

namespace MainSystem.Tests.Unit.Domain.Entities;

public class FlightRosterTests
{
    [Fact]
    public void AddPilot_WithValidPilot_ShouldAddSuccessfully()
    {
        // Arrange
        var flight = TestFlightFactory.CreateTestFlight();
        var roster = new FlightRoster(flight);
        var pilot = TestPilotFactory.CreateSeniorPilot();

        // Act
        var action = () => roster.AddPilot(pilot);

        // Assert
        action.Should().NotThrow();
        roster.Pilots.Should().Contain(pilot);
    }

    [Fact]
    public void AddPilot_WithWrongAircraftType_ShouldThrowException()
    {
        // Arrange
        var flight = TestFlightFactory.CreateTestFlight(AircraftType.Boeing737);
        var roster = new FlightRoster(flight);
        var pilot = TestPilotFactory.CreatePilot(aircraftType: AircraftType.AirbusA320);

        // Act
        var action = () => roster.AddPilot(pilot);

        // Assert
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("*uçak tipini kullanamaz*");
    }

    [Fact]
    public void AddAttendant_WithValidAttendant_ShouldAddSuccessfully()
    {
        // Arrange
        var flight = TestFlightFactory.CreateTestFlight();
        var roster = new FlightRoster(flight);
        var attendant = TestAttendantFactory.CreateChiefAttendant();

        // Act
        roster.AddAttendant(attendant);

        // Assert
        roster.Attendants.Should().Contain(attendant);
    }

    [Fact]
    public void AddPassenger_ExceedingCapacity_ShouldThrowException()
    {
        // Arrange
        var flight = TestFlightFactory.CreateSmallFlight(maxPassengers: 1);
        var roster = new FlightRoster(flight);
        var passenger1 = TestPassengerFactory.CreatePassenger();
        var passenger2 = TestPassengerFactory.CreatePassenger();

        // Act
        roster.AddPassenger(passenger1);
        var action = () => roster.AddPassenger(passenger2);

        // Assert
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("*kapasitesi dolu*");
    }
}