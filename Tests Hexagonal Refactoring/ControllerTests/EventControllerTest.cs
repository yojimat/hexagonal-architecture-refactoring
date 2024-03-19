using Hexagonal_Refactoring.Application.UseCases.Customer;
using Hexagonal_Refactoring.Application.UseCases.Event;
using Hexagonal_Refactoring.Application.UseCases.Partner;

namespace Tests_Hexagonal_Refactoring.ControllerTests;

public class EventControllerTest
{
    [Fact(DisplayName = "Should create an event")]
    public void TestCreate()
    {
        // Arrange
        var partnerRepository = new InMemoryPartnerRepository();
        var eventRepository = new InMemoryEventRepository();
        var customerRepository = new InMemoryCustomerRepository();
        var ticketRepository = new InMemoryTicketRepository();

        var createEventUseCase = new CreateEventUseCase(partnerRepository, new InMemoryEventRepository());
        var subscribeCustomerToEventUseCase =
            new SubscribeCustomerToEventUseCase(customerRepository, eventRepository, ticketRepository);

        var controller = new EventController(createEventUseCase, subscribeCustomerToEventUseCase);

        var newPartner = new NewPartnerDto("Partner", "02.308.322/0001-28", "partner@test.com");

        var partnerController = new PartnerController(new CreatePartnerUseCase(partnerRepository),
            new GetPartnerByIdUseCase(partnerRepository));

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
        var partnerRepository = new InMemoryPartnerRepository();
        var eventRepository = new InMemoryEventRepository();
        var customerRepository = new InMemoryCustomerRepository();
        var ticketRepository = new InMemoryTicketRepository();

        var createEventUseCase = new CreateEventUseCase(partnerRepository, eventRepository);
        var subscribeCustomerToEventUseCase =
            new SubscribeCustomerToEventUseCase(customerRepository, eventRepository, ticketRepository);

        var eventController = new EventController(createEventUseCase, subscribeCustomerToEventUseCase);

        var newPartner = new NewPartnerDto("Partner", "02.308.322/0001-28", "partner@test.com");

        var partnerController = new PartnerController(new CreatePartnerUseCase(partnerRepository),
            new GetPartnerByIdUseCase(partnerRepository));

        var partnerControllerOutput = partnerController.Create(newPartner) as ObjectResult;
        var partnerControllerOutputValue = partnerControllerOutput?.Value as CreatePartnerUseCase.Output;

        var eventDto = new NewEventDto("Disney on Ice", "2021-01-01", 100,
            partnerControllerOutputValue?.Id ?? throw new InvalidOperationException());

        var eventResult = eventController.Create(eventDto) as ObjectResult;
        var eventOutput = eventResult?.Value as CreateEventUseCase.Output;

        var customerController = new CustomerController(new CreateCustomerUseCase(customerRepository),
            new GetCustomerByIdUseCase(customerRepository));
        var customerResult =
            customerController.Create(new NewCustomerDto("customer name", "123.123.123-12", "customer@test.com")) as
                ObjectResult;

        var customerOutput = customerResult?.Value as CreateCustomerUseCase.Output;

        var subDto = new SubscribeDto(customerOutput?.Id ?? throw new ArgumentException());

        // Act
        var result = eventController.Subscribe(eventOutput?.EventId ?? throw new ArgumentException(), subDto);
        var exeResult = result as OkResult;

        // Assert
        Assert.NotNull(exeResult);
        Assert.Equal(StatusCodes.Status200OK, exeResult.StatusCode);
    }
}