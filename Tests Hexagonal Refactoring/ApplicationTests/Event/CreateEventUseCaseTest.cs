using Hexagonal_Refactoring.Application.UseCases.Event;
using Hexagonal_Refactoring.Application.UseCases.Partner;

namespace Tests_Hexagonal_Refactoring.ApplicationTests.Event;

public class CreateEventUseCaseTest
{
    [Fact(DisplayName = "Should create an event")]
    public void TestCreateEvent()
    {
        // Given
        const string expectedName = "Event Name";
        const int expectedTotalSpots = 100;
        var expectedDate = DateTime.Now.ToShortDateString();

        const string expectedCnpj = "11.545.127/0001-02";
        const string partnerExpectedName = "John Doe";
        const string expectedEmail = "test@test.com";

        var partnerRepository = new InMemoryPartnerRepository();
        CreatePartnerUseCase.Input partnerInput = new(expectedCnpj, expectedEmail, partnerExpectedName);
        CreatePartnerUseCase createPartnerUseCase = new(partnerRepository);
        var partnerOutput = createPartnerUseCase.Execute(partnerInput);

        CreateEventUseCase.Input eventInput = new(expectedName, expectedDate, expectedTotalSpots, partnerOutput.Id);

        var eventRepository = new InMemoryEventRepository();
        CreateEventUseCase useCase = new(partnerRepository, eventRepository);

        // When
        var eventOutput = useCase.Execute(eventInput);

        // Then
        Assert.NotNull(eventOutput);
        Assert.False(string.IsNullOrEmpty(eventOutput.EventId));
        Assert.Equal(eventOutput.PartnerId, partnerOutput.Id);
    }

    [Fact(DisplayName = "Should throw exception when partner not found")]
    public void TestCreateEventPartnerNotFound()
    {
        // Given
        const string expectedName = "Event Name";
        const int expectedTotalSpots = 100;
        var expectedDate = DateTime.Now.ToShortDateString();

        CreateEventUseCase.Input eventInput = new(expectedName, expectedDate, expectedTotalSpots,
            Guid.NewGuid().ToString());

        var eventRepository = new InMemoryEventRepository();
        var partnerRepository = new InMemoryPartnerRepository();
        CreateEventUseCase useCase = new(partnerRepository, eventRepository);

        // Then
        Assert.Throws<Exception>(() => useCase.Execute(eventInput));
    }
}