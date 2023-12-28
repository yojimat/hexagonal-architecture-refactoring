namespace Tests_Hexagonal_Refactoring.ControllerTests;

public class EventControllerTest
{
    private readonly Mock<IEventRepository> _eventRepositoryMock = new();
    private readonly Mock<ITicketRepository> _ticketRepositoryMock = new();
    private readonly Mock<IPartnerRepository> _partnerRepositoryMock = new();
    private readonly Mock<ICustomerRepository> _customerRepositoryMock = new();
    private readonly EventController _controller;
    private readonly EventDto _eventDto;
    private readonly Event _expectedEvent;
    private readonly Partner _disney;

    public EventControllerTest()
    {

        _disney = new Partner(1, "Disney", "456", "disney@gmail.com");
        _eventDto = new EventDto();
        _eventDto.SetDate("2021-01-01");
        _eventDto.SetName("Disney on Ice");
        _eventDto.SetTotalSpots(100);
        _eventDto.SetPartner(new PartnerDto(_disney.GetId()));

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
        var exeResultValue = exeResult.Value as EventDto;

        // Assert
        Assert.Equal(exeResult.StatusCode, StatusCodes.Status201Created);

        Assert.NotNull(exeResultValue);
        Assert.Equal(DateTime.Parse(exeResultValue.GetDate()!), _expectedEvent.GetDate());
        Assert.Equal(exeResultValue.GetTotalSpots(), _expectedEvent.GetTotalSpots());
        Assert.Equal(exeResultValue.GetName(), _expectedEvent.GetName());
    }

    [Fact(DisplayName = "Should buy an event ticket")]
    public void TestReserveTicket()
    {
        // Arrange  
        var johnDoe = new Customer(0, "John Doe", "123", "");
        var evnt = new Event(0, "Disney on Ice", DateTime.Now, 100, null);
        var ticket = new Ticket(1, johnDoe, _expectedEvent,
            TicketStatus.Pending, DateTime.Now,
            DateTime.Now + TimeSpan.FromTicks(100));

        var sub = new SubscribeDto();
        sub.SetCustomerId(johnDoe.GetId());

        _customerRepositoryMock.Setup(x =>
                                      x.FindById(It.Is<long>(idReceived => idReceived.Equals(0))))
            .Returns(johnDoe);

        _eventRepositoryMock.Setup(x =>
                           x.FindById(It.Is<long>(idReceived => idReceived.Equals(_expectedEvent.GetId()))))
            .Returns(_expectedEvent);

        _ticketRepositoryMock
            .Setup(x => x.FindByEventIdAndCustomerId(It.Is<long>(idReceived => idReceived.Equals(1)),
                It.Is<long>(idReceived => idReceived.Equals(1)))).Returns(ticket);

        // Act
        var result = _controller.Subscribe(evnt.GetId(), sub);
        var exeResult = result as ObjectResult;

        // Assert
        Assert.NotNull(exeResult);
        Assert.Equal(StatusCodes.Status200OK, exeResult.StatusCode);

        var actualEvent = _eventRepositoryMock.Object.FindById(evnt.GetId());
        Assert.Single(actualEvent.GetTickets()!); // This only makes sense if the tests is making a real request to the db
    }

    private EventController ControllerFactory()
    {
        var eventService = new EventService(_eventRepositoryMock.Object, _ticketRepositoryMock.Object);
        var partnerService = new PartnerService(_partnerRepositoryMock.Object);
        var customerService = new CustomerService(_customerRepositoryMock.Object);
        var controller = new EventController(eventService, partnerService, customerService);
        return controller;
    }

    private static Event TestEventFactory(EventDto evnt)
    {
        return new Event(evnt.GetId(), evnt.GetName(), DateTime.Parse(evnt.GetDate()!),
            evnt.GetTotalSpots(), new HashSet<Ticket>());
    }
}
