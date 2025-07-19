using Xunit;
using FluentAssertions;
using MainSystem.Domain.ValueObjects;
using MainSystem.Domain.Entities;
using MainSystem.Tests.Helpers.TestData;

namespace MainSystem.Tests.BlackBox.Input;

public class InputValidationTests
{
    [Theory]
    [InlineData("TK1234", true)]     // Valid Turkish Airlines
    [InlineData("AA9999", true)]     // Valid American Airlines
    [InlineData("BA0001", true)]     // Valid British Airways
    [InlineData("LH1234", true)]     // Valid Lufthansa
    [InlineData("", false)]          // Empty
    [InlineData("TK12345", false)]   // Too long
    [InlineData("T1234", false)]     // Too short
    [InlineData("tk1234", false)]    // Lowercase
    [InlineData("12TK34", false)]    // Wrong format
    [InlineData("TK12A4", false)]    // Letter in number part
    [InlineData("1K1234", false)]    // Number in airline code
    public void FlightNumber_InputValidation(string input, bool shouldBeValid)
    {
        // Act & Assert
        if (shouldBeValid)
        {
            var action = () => new FlightNumber(input);
            action.Should().NotThrow();
        }
        else
        {
            var action = () => new FlightNumber(input);
            action.Should().Throw<ArgumentException>();
        }
    }

    [Theory]
    [InlineData("IST", true)]        // Valid airport code
    [InlineData("JFK", true)]        // Valid airport code
    [InlineData("LHR", true)]        // Valid airport code
    [InlineData("", false)]          // Empty
    [InlineData("IS", false)]        // Too short
    [InlineData("ISTB", false)]      // Too long
    [InlineData("ist", false)]       // Lowercase
    [InlineData("I2T", false)]       // Contains number
    [InlineData("I-T", false)]       // Contains special char
    public void AirportCode_InputValidation(string input, bool shouldBeValid)
    {
        // Act & Assert
        if (shouldBeValid)
        {
            var action = () => new AirportCode(input);
            action.Should().NotThrow();
        }
        else
        {
            var action = () => new AirportCode(input);
            action.Should().Throw<ArgumentException>();
        }
    }

    [Theory]
    [InlineData("1A", true)]         // Valid seat
    [InlineData("10F", true)]        // Valid seat
    [InlineData("99Z", true)]        // Valid seat
    [InlineData("", false)]          // Empty
    [InlineData("A1", false)]        // Wrong format
    [InlineData("1", false)]         // Missing letter
    [InlineData("A", false)]         // Missing number
    [InlineData("0A", false)]        // Row cannot be 0
    [InlineData("-1A", false)]       // Negative row
    [InlineData("11", false)]        // Missing column
    public void SeatNumber_InputValidation(string input, bool shouldBeValid)
    {
        // Act & Assert
        if (shouldBeValid)
        {
            var action = () => SeatNumber.Parse(input);
            action.Should().NotThrow();
        }
        else
        {
            var action = () => SeatNumber.Parse(input);
            action.Should().Throw<Exception>();
        }
    }

    [Theory]
    [InlineData(5, true)]            // Valid 5 minutes
    [InlineData(600, true)]          // Valid 10 hours
    [InlineData(1440, true)]         // Valid 24 hours
    [InlineData(4, false)]           // Too short
    [InlineData(0, false)]           // Zero duration
    [InlineData(-30, false)]         // Negative duration
    public void FlightDuration_InputValidation(int minutes, bool shouldBeValid)
    {
        // Act & Assert
        if (shouldBeValid)
        {
            var action = () => new FlightDuration(TimeSpan.FromMinutes(minutes));
            action.Should().NotThrow();
        }
        else
        {
            var action = () => new FlightDuration(TimeSpan.FromMinutes(minutes));
            action.Should().Throw<ArgumentException>();
        }
    }
}