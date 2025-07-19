using Xunit;
using FluentAssertions;
using MainSystem.Domain.ValueObjects;

namespace MainSystem.Tests.Unit.Domain.ValueObjects;

public class FlightNumberTests
{
    [Theory]
    [InlineData("TK1234")]
    [InlineData("AA9999")]
    [InlineData("LH0001")]
    public void Constructor_WithValidFormat_ShouldCreateFlightNumber(string validNumber)
    {
        // Act
        var flightNumber = new FlightNumber(validNumber);

        // Assert
        flightNumber.Value.Should().Be(validNumber);
    }

    [Theory]
    [InlineData("123TK")]    // Wrong format
    [InlineData("T1234")]    // Too short
    [InlineData("TK12345")]  // Too long
    [InlineData("tk1234")]   // Lowercase
    [InlineData("")]         // Empty
    [InlineData(null)]       // Null
    public void Constructor_WithInvalidFormat_ShouldThrowException(string invalidNumber)
    {
        // Act
        var action = () => new FlightNumber(invalidNumber);

        // Assert
        action.Should().Throw<ArgumentException>()
            .WithMessage("*AANNNN format*");
    }

    [Fact]
    public void ImplicitConversion_ToString_ShouldWork()
    {
        // Arrange
        var flightNumber = new FlightNumber("TK1234");

        // Act
        string result = flightNumber;

        // Assert
        result.Should().Be("TK1234");
    }
}