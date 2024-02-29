using Hexagonal_Refactoring.Application.UseCases;

namespace Tests_Hexagonal_Refactoring.ApplicationTests;

public class CreateEventUseCaseTest
{
    [Fact(DisplayName = "Should create an event")]
    public void TestCreateEvent()
    {
        // Given
        const string expectedName = "Event Name";
        const int expectedTotalSpots = 100;
        const long partnerId = 1;
        var expectedDate = DateTime.Now;

        var mockPartner = new Partner(partnerId, "", "", "");
        var mockEvent = new Event(1, expectedName, expectedDate, expectedTotalSpots, new HashSet<Ticket>());
        mockEvent.SetPartner(mockPartner);

        CreateEventUseCase.Input createInput = new(expectedName,
            expectedDate.ToShortDateString(),
            expectedTotalSpots,
            partnerId);

        Mock<ITicketRepository> ticketRepository = new();
        Mock<IEventRepository> eventRepository = new();
        eventRepository.Setup(x => x.Save(It.Is<Event>(e => e.GetName() == expectedName &&
                                e.GetTotalSpots() == expectedTotalSpots)))
                        .Returns(mockEvent);

        Mock<IPartnerRepository> partnerRepository = new();
        partnerRepository.Setup(x => x.FindById(It.Is<long>(id => id == partnerId)))
            .Returns(mockPartner);

        var partnerService = new PartnerService(partnerRepository.Object);
        var eventService = new EventService(eventRepository.Object, ticketRepository.Object);

        // When
        CreateEventUseCase useCase = new(partnerService, eventService);
        var output = useCase.Execute(createInput);

        // Then
        Assert.NotNull(output);
        Assert.Equal(mockEvent.GetId(), output.Id);
        Assert.Equal(mockEvent.GetPartner().GetId(), output.Id);
    }
}