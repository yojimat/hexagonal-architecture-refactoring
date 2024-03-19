using System.ComponentModel.DataAnnotations;
using Hexagonal_Refactoring.Application.UseCases.Customer;
using Hexagonal_Refactoring.Application.UseCases.Event;
using Hexagonal_Refactoring.Application.UseCases.Partner;

namespace Tests_Hexagonal_Refactoring.ApplicationTests.Event;

public class SubscribeCustomerToEventUseCaseTest
{
    [Fact(DisplayName = "Should subscribe a customer to an event")]
    public void TestSubscribeCustomerToEvent()
    {
        // Given
        var partnerRepository = new InMemoryPartnerRepository();
        var eventRepository = new InMemoryEventRepository();
        var customerRepository = new InMemoryCustomerRepository();
        var ticketRepository = new InMemoryTicketRepository();

        CreatePartnerUseCase.Input partnerInput = new("11.545.127/0001-02", "partner@test.com", "partner name");
        var partnerUseCase = new CreatePartnerUseCase(partnerRepository);
        var partnerOutput = partnerUseCase.Execute(partnerInput);

        CreateEventUseCase.Input eventInput = new("Event 1",
            DateTime.Now.ToShortDateString(), 10, partnerOutput.Id);
        var eventUseCase = new CreateEventUseCase(partnerRepository, eventRepository);
        var eventOutput = eventUseCase.Execute(eventInput);

        CreateCustomerUseCase.Input customerInput = new("123.123.123-12", "customer@test.com", "customer name");
        var customerUseCase = new CreateCustomerUseCase(customerRepository);
        var customerOutput = customerUseCase.Execute(customerInput);

        SubscribeCustomerToEventUseCase.Input subscribeInput = new(customerOutput.Id, eventOutput.EventId);
        var subscribeCustomerToEventUseCase =
            new SubscribeCustomerToEventUseCase(customerRepository, eventRepository, ticketRepository);

        //when
        var subscribeOutput = subscribeCustomerToEventUseCase.Execute(subscribeInput);

        //then
        Assert.False(string.IsNullOrEmpty(subscribeOutput.TicketId));
    }

    [Fact(DisplayName = "Shouldn't subscribe a customer to an event if the event is sold out")]
    public void TestWhenEventIsSoldOut()
    {
        // Given
        var partnerRepository = new InMemoryPartnerRepository();
        var eventRepository = new InMemoryEventRepository();
        var customerRepository = new InMemoryCustomerRepository();
        var ticketRepository = new InMemoryTicketRepository();

        CreatePartnerUseCase.Input partnerInput = new("11.545.127/0001-02", "partner@test.com", "partner name");
        var partnerUseCase = new CreatePartnerUseCase(partnerRepository);
        var partnerOutput = partnerUseCase.Execute(partnerInput);

        CreateEventUseCase.Input eventInput = new("Event 1",
            DateTime.Now.ToShortDateString(), 0, partnerOutput.Id);
        var eventUseCase = new CreateEventUseCase(partnerRepository, eventRepository);
        var eventOutput = eventUseCase.Execute(eventInput);

        CreateCustomerUseCase.Input customerInput = new("123.123.123-12", "customer@test.com", "customer name");
        var customerUseCase = new CreateCustomerUseCase(customerRepository);
        var customerOutput = customerUseCase.Execute(customerInput);

        SubscribeCustomerToEventUseCase.Input subscribeInput = new(customerOutput.Id, eventOutput.EventId);
        var subscribeCustomerToEventUseCase =
            new SubscribeCustomerToEventUseCase(customerRepository, eventRepository, ticketRepository);

        //then
        var ex = Assert.Throws<InvalidOperationException>(() =>
            subscribeCustomerToEventUseCase.Execute(subscribeInput));
        Assert.Equal("Event sold out", ex.Message);
    }

    [Fact(DisplayName = "Shouldn't subscribe a customer to an event if the customer is already subscribed")]
    public void TestWhenAlreadySubscribed()
    {
        // Given
        var partnerRepository = new InMemoryPartnerRepository();
        var eventRepository = new InMemoryEventRepository();
        var customerRepository = new InMemoryCustomerRepository();
        var ticketRepository = new InMemoryTicketRepository();

        CreatePartnerUseCase.Input partnerInput = new("11.545.127/0001-02", "partner@test.com", "partner name");
        var partnerUseCase = new CreatePartnerUseCase(partnerRepository);
        var partnerOutput = partnerUseCase.Execute(partnerInput);

        CreateEventUseCase.Input eventInput = new("Event 1",
            DateTime.Now.ToShortDateString(), 1, partnerOutput.Id);
        var eventUseCase = new CreateEventUseCase(partnerRepository, eventRepository);
        var eventOutput = eventUseCase.Execute(eventInput);

        CreateCustomerUseCase.Input customerInput = new("123.123.123-12", "customer@test.com", "customer name");
        var customerUseCase = new CreateCustomerUseCase(customerRepository);
        var customerOutput = customerUseCase.Execute(customerInput);

        SubscribeCustomerToEventUseCase.Input subscribeInput = new(customerOutput.Id, eventOutput.EventId);
        var subscribeCustomerToEventUseCase =
            new SubscribeCustomerToEventUseCase(customerRepository, eventRepository, ticketRepository);

        // When
        // Buy one ticket
        subscribeCustomerToEventUseCase.Execute(subscribeInput);

        //then
        var ex = Assert.Throws<InvalidOperationException>(() =>
            subscribeCustomerToEventUseCase.Execute(subscribeInput));
        Assert.Equal("Email already registered", ex.Message);
    }

    [Fact(DisplayName = "Shouldn't subscribe a customer to an event if the customer is not found")]
    public void TestSubscriberNotFound()
    {
        // Given
        var partnerRepository = new InMemoryPartnerRepository();
        var eventRepository = new InMemoryEventRepository();
        var customerRepository = new InMemoryCustomerRepository();
        var ticketRepository = new InMemoryTicketRepository();

        CreatePartnerUseCase.Input partnerInput = new("11.545.127/0001-02", "partner@test.com", "partner name");
        var partnerUseCase = new CreatePartnerUseCase(partnerRepository);
        var partnerOutput = partnerUseCase.Execute(partnerInput);

        CreateEventUseCase.Input eventInput = new("Event 1",
            DateTime.Now.ToShortDateString(), 0, partnerOutput.Id);
        var eventUseCase = new CreateEventUseCase(partnerRepository, eventRepository);
        var eventOutput = eventUseCase.Execute(eventInput);

        SubscribeCustomerToEventUseCase.Input subscribeInput = new(Guid.NewGuid().ToString(), eventOutput.EventId);
        var subscribeCustomerToEventUseCase =
            new SubscribeCustomerToEventUseCase(customerRepository, eventRepository, ticketRepository);

        //then
        var ex = Assert.Throws<ValidationException>(() => subscribeCustomerToEventUseCase.Execute(subscribeInput));
        Assert.Equal("Customer not found", ex.Message);
    }

    [Fact(DisplayName = "Shouldn't subscribe a customer to an event if the event is not found")]
    public void TestEventNotFound()
    {
        // Given
        var eventRepository = new InMemoryEventRepository();
        var customerRepository = new InMemoryCustomerRepository();
        var ticketRepository = new InMemoryTicketRepository();

        SubscribeCustomerToEventUseCase.Input
            subscribeInput = new(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
        var subscribeCustomerToEventUseCase =
            new SubscribeCustomerToEventUseCase(customerRepository, eventRepository, ticketRepository);

        //then
        var ex = Assert.Throws<ValidationException>(() => subscribeCustomerToEventUseCase.Execute(subscribeInput));
        Assert.Equal("Event not found", ex.Message);
    }
}