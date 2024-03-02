using System.ComponentModel.DataAnnotations;
using Hexagonal_Refactoring.Application.UseCases;

namespace Tests_Hexagonal_Refactoring.ApplicationTests;

public class SubscribeCustomerToEventUseCaseTest
{
    [Fact(DisplayName = "Should subscribe a customer to an event")]
    public void TestSubscribeCustomerToEvent()
    {
        // Given
        const long expectedCustomerId = 1;
        const long expectedEventId = 1;

        var mockEvent = new Event(1, "Event 1", DateTime.Now, 10, new HashSet<Ticket>());
        var mockCustomer = new Customer(1, "John Doe", "123.456.789-00", "");

        SubscribeCustomerToEventUseCase.Input subscribeInput = new(expectedCustomerId, expectedEventId);

        Mock<ICustomerService> customerServiceMock = new();
        customerServiceMock.Setup(x =>
                       x.FindById(It.Is<long>(idReceived =>
                                          idReceived.Equals(expectedCustomerId))))
            .Returns(mockCustomer);

        Mock<IEventService> eventServiceMock = new();
        eventServiceMock.Setup(x =>
                       x.FindById(It.Is<long>(idReceived =>
                                          idReceived.Equals(expectedEventId))))
            .Returns(mockEvent);

        eventServiceMock.Setup(x =>
                       x.FindTicketByEventIdAndCustomerId(It.Is<long>(eventId =>
                                          eventId.Equals(expectedEventId)),
                                          It.Is<long>(customerId =>
                                                                 customerId.Equals(expectedCustomerId))))
            .Returns((Ticket?)null);

        //when
        SubscribeCustomerToEventUseCase useCase = new(customerServiceMock.Object, eventServiceMock.Object);
        useCase.Execute(subscribeInput);

        //then
        eventServiceMock.Verify(x => x.Save(It.Is<Event>(e =>
                                                     e.GetTickets()!.Count == 1)), Times.Once);
    }

    [Fact(DisplayName = "Shouldn't subscribe a customer to an event if the event is sold out")]
    public void TestWhenEventIsSoldOut()
    {
        // Given
        const long expectedCustomerId = 1;
        const long expectedEventId = 1;

        var mockEvent = new Event(1, "Event 1", DateTime.Now, 0, new HashSet<Ticket>());
        var mockCustomer = new Customer(1, "John Doe", "123.456.789-00", "");

        SubscribeCustomerToEventUseCase.Input subscribeInput = new(expectedCustomerId, expectedEventId);

        Mock<ICustomerService> customerServiceMock = new();
        customerServiceMock.Setup(x =>
                       x.FindById(It.Is<long>(idReceived =>
                                          idReceived.Equals(expectedCustomerId))))
            .Returns(mockCustomer);

        Mock<IEventService> eventServiceMock = new();
        eventServiceMock.Setup(x =>
                       x.FindById(It.Is<long>(idReceived =>
                                          idReceived.Equals(expectedEventId))))
            .Returns(mockEvent);

        eventServiceMock.Setup(x =>
                       x.FindTicketByEventIdAndCustomerId(It.Is<long>(eventId =>
                                          eventId.Equals(expectedEventId)),
                                          It.Is<long>(customerId =>
                                                                 customerId.Equals(expectedCustomerId))))
            .Returns((Ticket?)null);

        //when
        SubscribeCustomerToEventUseCase useCase = new(customerServiceMock.Object, eventServiceMock.Object);

        //then
        var ex = Assert.Throws<ValidationException>(() => useCase.Execute(subscribeInput));
        Assert.Equal("Event sold out", ex.Message);
    }

    [Fact(DisplayName = "Shouldn't subscribe a customer to an event if the customer is already subscribed")]
    public void TestWhenAlreadySubscribed()
    {
        // Given
        const long expectedCustomerId = 1;
        const long expectedEventId = 1;

        var mockEvent = new Event(1, "Event 1", DateTime.Now, 10, new HashSet<Ticket>());
        var mockCustomer = new Customer(1, "John Doe", "123.456.789-00", "");

        SubscribeCustomerToEventUseCase.Input subscribeInput = new(expectedCustomerId, expectedEventId);

        Mock<ICustomerService> customerServiceMock = new();
        customerServiceMock.Setup(x =>
                              x.FindById(It.Is<long>(idReceived =>
                                                                       idReceived.Equals(expectedCustomerId))))
            .Returns(mockCustomer);

        Mock<IEventService> eventServiceMock = new();
        eventServiceMock.Setup(x =>
                              x.FindById(It.Is<long>(idReceived =>
                                                                       idReceived.Equals(expectedEventId))))
            .Returns(mockEvent);

        eventServiceMock.Setup(x =>
                              x.FindTicketByEventIdAndCustomerId(It.Is<long>(eventId =>
                                                                       eventId.Equals(expectedEventId)),
                                                                       It.Is<long>(customerId =>
                                                                       customerId.Equals(expectedCustomerId))))
            .Returns(new Ticket());

        //when
        SubscribeCustomerToEventUseCase useCase = new(customerServiceMock.Object, eventServiceMock.Object);

        //then
        var ex = Assert.Throws<ValidationException>(() => useCase.Execute(subscribeInput));
        Assert.Equal("Email already registered", ex.Message);
    }

    [Fact(DisplayName = "Shouldn't subscribe a customer to an event if the customer is not found")]
    public void TestSubscriberNotFound()
    {
        // Given
        const long expectedCustomerId = 1;
        const long expectedEventId = 1;

        var mockEvent = new Event(1, "Event 1", DateTime.Now, 10, new HashSet<Ticket>());

        SubscribeCustomerToEventUseCase.Input subscribeInput = new(expectedCustomerId, expectedEventId);

        Mock<ICustomerService> customerServiceMock = new();
        customerServiceMock.Setup(x =>
                                         x.FindById(It.Is<long>(idReceived =>
                                           idReceived.Equals(expectedCustomerId))))
            .Returns((Customer?)null);

        Mock<IEventService> eventServiceMock = new();
        eventServiceMock.Setup(x =>
                                         x.FindById(It.Is<long>(idReceived =>
                                           idReceived.Equals(expectedEventId))))
            .Returns(mockEvent);

        //when
        SubscribeCustomerToEventUseCase useCase = new(customerServiceMock.Object, eventServiceMock.Object);

        //then
        var ex = Assert.Throws<ValidationException>(() => useCase.Execute(subscribeInput));
        Assert.Equal("Customer not found", ex.Message);
    }

    [Fact(DisplayName = "Shouldn't subscribe a customer to an event if the event is not found")]
    public void TestEventNotFound()
    {
        // Given
        const long expectedCustomerId = 1;
        const long expectedEventId = 1;

        SubscribeCustomerToEventUseCase.Input subscribeInput = new(expectedCustomerId, expectedEventId);

        Mock<ICustomerService> customerServiceMock = new();

        Mock<IEventService> eventServiceMock = new();
        eventServiceMock.Setup(x =>
                                              x.FindById(It.Is<long>(idReceived =>
                                                  idReceived.Equals(expectedEventId))))
            .Returns((Event?)null);

        //when
        SubscribeCustomerToEventUseCase useCase = new(customerServiceMock.Object, eventServiceMock.Object);

        //then
        var ex = Assert.Throws<ValidationException>(() => useCase.Execute(subscribeInput));
        Assert.Equal("Event not found", ex.Message);
    }
}
