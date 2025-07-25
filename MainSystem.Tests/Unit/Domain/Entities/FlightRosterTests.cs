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
        
        // Add a senior pilot first
        var seniorPilot = TestPilotFactory.CreateSeniorPilot();
        roster.AddPilot(seniorPilot);
        
        // Act - Add a junior pilot (this should work since we have both types)
        var juniorPilot = TestPilotFactory.CreateJuniorPilot();
        var action = () => roster.AddPilot(juniorPilot);

        // Assert
        action.Should().NotThrow();
        roster.Pilots.Should().Contain(seniorPilot);
        roster.Pilots.Should().Contain(juniorPilot);
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
        
        // First add 4 regular attendants (minimum requirement)
        for (int i = 0; i < 4; i++)
        {
            roster.AddAttendant(TestAttendantFactory.CreateRegularAttendant());
        }
        
        // Act - Now add a chief attendant (should work since we have the required regulars)
        var chiefAttendant = TestAttendantFactory.CreateChiefAttendant();
        var action = () => roster.AddAttendant(chiefAttendant);

        // Assert
        action.Should().NotThrow();
        roster.Attendants.Should().Contain(chiefAttendant);
    }

    [Fact]
    public void AddPassenger_ExceedingCapacity_ShouldThrowException()
    {
        // Arrange
        var flight = TestFlightFactory.CreateSmallFlight(maxPassengers: 1);
        var roster = new FlightRoster(flight);
        
        // Create passengers with the correct flight number to match the flight
        var passenger1 = TestPassengerFactory.CreatePassenger("TK5678"); // Match the flight number from CreateSmallFlight
        var passenger2 = TestPassengerFactory.CreatePassenger("TK5678"); // Match the flight number from CreateSmallFlight

        // Act
        roster.AddPassenger(passenger1);
        var action = () => roster.AddPassenger(passenger2);

        // Assert
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("*kapasitesi dolu*");
    }
}