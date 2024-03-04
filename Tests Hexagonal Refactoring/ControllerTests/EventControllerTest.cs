using Hexagonal_Refactoring.Application.UseCases;
using Hexagonal_Refactoring.Models;
using Hexagonal_Refactoring.Repositories;

namespace Tests_Hexagonal_Refactoring.ControllerTests;

public class EventControllerTest
{
    private readonly EventController _controller;
    private readonly Mock<ICustomerRepository> _customerRepositoryMock = new();
    private readonly Partner _disney;
    private readonly NewEventDto _eventDto;
    private readonly Mock<IEventRepository> _eventRepositoryMock = new();
    private readonly Event _expectedEvent;
    private readonly Mock<IPartnerRepository> _partnerRepositoryMock = new();
    private readonly Mock<ITicketRepository> _ticketRepositoryMock = new();

    public EventControllerTest()
    {
        _disney = new Partner(1, "Disney", "456", "disney@gmail.com");
        _eventDto = new NewEventDto("Disney on Ice", "2021-01-01", 100, _disney);

        _expectedEvent = TestEventFactory(_eventDto);
        _controller = ControllerFactory();
    }

    [Fact(DisplayName = "Should create an event")]
    public void TestCreate()
    {
        // Arrange
        _eventRepositoryMock.Setup(x =>
                x.Save(It.Is<Event>(cReceived => cReceived.Equals(_expectedEvent))))
            .Returns(_expectedEvent);

        _partnerRepositoryMock.Setup(x =>
                x.FindById(It.Is<long>(idReceived => idReceived.Equals(_disney.GetId()))))
            .Returns(_disney);

        // Act
        var result = _controller.Create(_eventDto);
        var exeResult = result as ObjectResult;

        Assert.NotNull(exeResult);
        var exeResultValue = exeResult.Value as NewEventDto;

        // Assert
        Assert.Equal(exeResult.StatusCode, StatusCodes.Status201Created);

        Assert.NotNull(exeResultValue);
        Assert.Equal(DateTime.Parse(exeResultValue.Date!), _expectedEvent.GetDate());
        Assert.Equal(exeResultValue.TotalSpots, _expectedEvent.GetTotalSpots());
        Assert.Equal(exeResultValue.Name, _expectedEvent.GetName());
    }

    [Fact(DisplayName = "Should buy an event ticket")]
    public void TestReserveTicket()
    {
        // Arrange  
        var johnDoe = new Customer(0, "John Doe", "123", "");
        var evnt = new Event(0, "Disney on Ice", DateTime.Now, 100, null);

        var sub = new SubscribeDto(johnDoe.GetId());

        _customerRepositoryMock.Setup(x =>
                x.FindById(It.Is<long>(idReceived => idReceived.Equals(0))))
            .Returns(johnDoe);

        _eventRepositoryMock.Setup(x =>
                x.FindById(It.Is<long>(idReceived => idReceived.Equals(_expectedEvent.GetId()))))
            .Returns(_expectedEvent);

        _ticketRepositoryMock
            .Setup(x => x.FindByEventIdAndCustomerId(It.Is<long>(idReceived => idReceived.Equals(0)),
                It.Is<long>(idReceived => idReceived.Equals(0)))).Returns((Ticket?)null);

        // Act
        var result = _controller.Subscribe(evnt.GetId(), sub);
        var exeResult = result as OkResult;

        // Assert
        Assert.NotNull(exeResult);
        Assert.Equal(StatusCodes.Status200OK, exeResult.StatusCode);
    }

    private EventController ControllerFactory()
    {
        var eventService = new EventService(_eventRepositoryMock.Object, _ticketRepositoryMock.Object);
        var partnerService = new PartnerService(_partnerRepositoryMock.Object);
        var customerService = new CustomerService(_customerRepositoryMock.Object);
        var createEventUseCase = new CreateEventUseCase(partnerService, eventService);
        var subscribeCustomerToEventUseCase = new SubscribeCustomerToEventUseCase(customerService, eventService);

        var controller = new EventController(createEventUseCase, subscribeCustomerToEventUseCase);
        return controller;
    }

    private static Event TestEventFactory(NewEventDto evnt)
    {
        return new Event(0, evnt.Name, DateTime.Parse(evnt.Date!),
            evnt.TotalSpots, new HashSet<Ticket>());
    }
}