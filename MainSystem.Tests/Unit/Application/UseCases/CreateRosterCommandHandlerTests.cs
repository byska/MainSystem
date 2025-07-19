using Xunit;
using FluentAssertions;
using MainSystem.Application.Abstraction;
using MainSystem.Application.UseCases.FlightRosterUseCases.Commands;
using MainSystem.Domain.Entities;
using MainSystem.Domain.Services.Builders;
using MainSystem.Domain.ValueObjects;
using MainSystem.Tests.Helpers.TestData;
using Moq;

namespace MainSystem.Tests.Unit.Application.UseCases;

public class CreateRosterCommandHandlerTests
{
    private readonly Mock<IFlightInfoRepository> _flightRepo;
    private readonly Mock<IPassengerRepository> _passengerRepo;
    private readonly Mock<IPilotPoolRepository> _pilotRepo;
    private readonly Mock<ICabinCrewPoolRepository> _cabinRepo;
    private readonly Mock<IFlightRosterRepository> _rosterRepo;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly FlightRosterBuilder _builder;
    private readonly CreateRosterCommandHandler _handler;

    public CreateRosterCommandHandlerTests()
    {
        _flightRepo = new Mock<IFlightInfoRepository>();
        _passengerRepo = new Mock<IPassengerRepository>();
        _pilotRepo = new Mock<IPilotPoolRepository>();
        _cabinRepo = new Mock<ICabinCrewPoolRepository>();
        _rosterRepo = new Mock<IFlightRosterRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _builder = new FlightRosterBuilder();

        _handler = new CreateRosterCommandHandler(
            _flightRepo.Object,
            _passengerRepo.Object,
            _pilotRepo.Object,
            _cabinRepo.Object,
            _rosterRepo.Object,
            _builder,
            _unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_WithValidFlightNumber_ShouldCreateRoster()
    {
        // Arrange
        var flightNumber = "TK1234";
        var flight = TestFlightFactory.CreateTestFlight();
        var pilots = TestPilotFactory.CreateValidPilotCrew();
        var attendants = TestAttendantFactory.CreateValidCabinCrew();
        var passengers = TestPassengerFactory.CreatePassengerList(10);

        _flightRepo.Setup(x => x.GetByFlightAsync(It.IsAny<FlightNumber>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(flight);
        _passengerRepo.Setup(x => x.ListByFlightAsync(It.IsAny<FlightNumber>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(passengers);
        _pilotRepo.Setup(x => x.ListAsync(It.IsAny<ISpecification<PilotMember>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(pilots);
        _cabinRepo.Setup(x => x.ListAsync(It.IsAny<ISpecification<CabinAttendantMember>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(attendants);

        var command = new CreateRosterCommand(flightNumber);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        _rosterRepo.Verify(x => x.AddAsync(It.IsAny<FlightRoster>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistentFlight_ShouldThrowException()
    {
        // Arrange
        _flightRepo.Setup(x => x.GetByFlightAsync(It.IsAny<FlightNumber>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Flight?)null);

        var command = new CreateRosterCommand("XX9999");

        // Act
        var action = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*bulunamadı*");
    }
}