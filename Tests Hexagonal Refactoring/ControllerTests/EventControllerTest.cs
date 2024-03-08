using Hexagonal_Refactoring.Models;
using Hexagonal_Refactoring.Repositories;
using Customer = Hexagonal_Refactoring.Models.Customer;
using Event = Hexagonal_Refactoring.Models.Event;
using ICustomerRepository = Hexagonal_Refactoring.Repositories.ICustomerRepository;
using IEventRepository = Hexagonal_Refactoring.Repositories.IEventRepository;

namespace Tests_Hexagonal_Refactoring.ControllerTests;

public class EventControllerTest
{
    private readonly Mock<ICustomerRepository> _customerRepositoryMock = new();
    private readonly Mock<IEventRepository> _eventRepositoryMock = new();
    private readonly Mock<ITicketRepository> _ticketRepositoryMock = new();

    [Fact(DisplayName = "Should create an event")]
    public void TestCreate()
    {
        // Arrange
        var eventService = new EventService(_eventRepositoryMock.Object, _ticketRepositoryMock.Object);
        var customerService = new CustomerService(_customerRepositoryMock.Object);
        var partnerRepository = new InMemoryPartnerRepository();

        var createEventUseCase = new CreateEventUseCase(partnerRepository, new InMemoryEventRepository());
        var subscribeCustomerToEventUseCase = new SubscribeCustomerToEventUseCase(customerService, eventService);

        var controller = new EventController(createEventUseCase, subscribeCustomerToEventUseCase);

        var newPartner = new NewPartnerDto("Partner", "02.308.322/0001-28", "partner@test.com");

        var partnerController = new PartnerController(new CreatePartnerUseCase(partnerRepository), new GetPartnerByIdUseCase(partnerRepository));

        var partnerControllerOutput = partnerController.Create(newPartner) as ObjectResult;
        var partnerControllerOutputValue = partnerControllerOutput?.Value as CreatePartnerUseCase.Output;

        var eventDto = new NewEventDto("Disney on Ice", "2021-01-01", 100,
            partnerControllerOutputValue?.Id ?? throw new InvalidOperationException());

        // Act
        var result = controller.Create(eventDto);
        var exeResult = result as ObjectResult;

        Assert.NotNull(exeResult);
        var exeResultValue = exeResult.Value as CreateEventUseCase.Output;

        // Assert
        Assert.Equal(exeResult.StatusCode, StatusCodes.Status201Created);

        Assert.NotNull(exeResultValue);
        Assert.NotNull(exeResultValue.EventId);
        Assert.False(string.IsNullOrEmpty(exeResultValue.EventId));
        Assert.Equal(exeResultValue.PartnerId, partnerControllerOutputValue.Id);
    }

    [Fact(DisplayName = "Should buy an event ticket")]
    public void TestReserveTicket()
    {
        // Arrange  

        var eventService = new EventService(_eventRepositoryMock.Object, _ticketRepositoryMock.Object);
        var customerService = new CustomerService(_customerRepositoryMock.Object);
        var partnerRepository = new InMemoryPartnerRepository();
        var eventRepository = new InMemoryEventRepository();

        var createEventUseCase = new CreateEventUseCase(partnerRepository, eventRepository);
        var subscribeCustomerToEventUseCase = new SubscribeCustomerToEventUseCase(customerService, eventService);

        var newPartner = new NewPartnerDto("Partner", "02.308.322/0001-28", "partner@test.com");

        var partnerController = new PartnerController(new CreatePartnerUseCase(partnerRepository), new GetPartnerByIdUseCase(partnerRepository));

        var partnerControllerOutput = partnerController.Create(newPartner) as ObjectResult;
        var partnerControllerOutputValue = partnerControllerOutput?.Value as CreatePartnerUseCase.Output;

        var eventDto = new NewEventDto("Disney on Ice", "2021-01-01", 100,
            partnerControllerOutputValue?.Id ?? throw new InvalidOperationException());

        var controller = new EventController(createEventUseCase, subscribeCustomerToEventUseCase);
        controller.Create(eventDto);

        var johnDoe = new Customer(0, "John Doe", "123", "");
        var evnt = new Event(0, "Disney on Ice", DateTime.Now, 100, null);

        var sub = new SubscribeDto(johnDoe.GetId());

        _customerRepositoryMock.Setup(x =>
                x.FindById(It.Is<long>(idReceived => idReceived.Equals(0))))
            .Returns(johnDoe);

        _ticketRepositoryMock
            .Setup(x => x.FindByEventIdAndCustomerId(It.Is<long>(idReceived => idReceived.Equals(0)),
                It.Is<long>(idReceived => idReceived.Equals(0)))).Returns((Ticket?)null);

        // Act
        var result = controller.Subscribe(evnt.GetId(), sub);
        var exeResult = result as OkResult;

        // Assert
        Assert.NotNull(exeResult);
        Assert.Equal(StatusCodes.Status200OK, exeResult.StatusCode);
    }
}