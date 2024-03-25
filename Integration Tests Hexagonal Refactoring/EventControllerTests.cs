using Hexagonal_Refactoring.Application.UseCases.Customer;
using Hexagonal_Refactoring.Application.UseCases.Event;
using Hexagonal_Refactoring.Application.UseCases.Partner;

namespace Integration_Tests_Hexagonal_Refactoring;

[Collection("Database tests")]
public class EventControllerTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private const string Url = "/api/events";

    [Fact]
    public async Task PostEvent()
    {
        // Arrange
        var client = factory.CreateClient();
        NewPartnerDto partnerDto = new("John Doe", "11.545.127/0001-02", "johndoe@gmail.com");
        var partnerResponse = await client.PostAsJsonAsync("api/partners", partnerDto);
        partnerResponse.EnsureSuccessStatusCode(); // Status Code 200-299
        var contentString = await partnerResponse.Content.ReadAsStringAsync();
        var partnerValue = JsonConvert.DeserializeObject<CreatePartnerUseCase.Output>(contentString);

        Assert.NotNull(partnerValue);
        NewEventDto eventDto = new("Event 1", "2022-12-31", 100, partnerValue.Id);

        // Act
        var response = await client.PostAsJsonAsync(Url, eventDto);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        contentString = await response.Content.ReadAsStringAsync();
        var responseValue = JsonConvert.DeserializeObject<CreateEventUseCase.Output>(contentString);

        Assert.NotNull(responseValue);
        Assert.Equal(eventDto.PartnerId, responseValue.PartnerId);
        Assert.False(string.IsNullOrEmpty(responseValue.EventId));
    }

    [Fact]
    public async Task SubscribeToEvent()
    {
        // Arrange
        var client = factory.CreateClient();
        NewPartnerDto partnerDto = new("John Doe", "11.545.227/0001-02", "johndoe2@gmail.com");
        var partnerResponse = await client.PostAsJsonAsync("api/partners", partnerDto);
        partnerResponse.EnsureSuccessStatusCode(); // Status Code 200-299
        var contentString = await partnerResponse.Content.ReadAsStringAsync();
        var partnerValue = JsonConvert.DeserializeObject<CreatePartnerUseCase.Output>(contentString);

        Assert.NotNull(partnerValue);
        NewEventDto eventDto = new("Event 1", "2022-12-31", 100, partnerValue.Id);
        var createResponse = await client.PostAsJsonAsync(Url, eventDto);
        createResponse.EnsureSuccessStatusCode(); // Status Code 200-299
        contentString = await createResponse.Content.ReadAsStringAsync();
        var createValue = JsonConvert.DeserializeObject<CreateEventUseCase.Output>(contentString);

        var customerDto = new NewCustomerDto("John Doe", "124.456.789-01", "johndoe2@gmail.com");
        var customerResponse = await client.PostAsJsonAsync("api/customers", customerDto);
        createResponse.EnsureSuccessStatusCode();// Status Code 200-299
        contentString = await customerResponse.Content.ReadAsStringAsync();
        var customerValue = JsonConvert.DeserializeObject<CreateCustomerUseCase.Output>(contentString);

        // Act
        Assert.NotNull(createValue);
        Assert.NotNull(customerValue);
        var response = await client.PostAsJsonAsync($"{Url}/{createValue.EventId}/subscribe", new SubscribeDto(customerValue.Id));
            
        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        contentString = await response.Content.ReadAsStringAsync();
        var responseValue = JsonConvert.DeserializeObject<SubscribeCustomerToEventUseCase.Output>(contentString);

        Assert.NotNull(responseValue);
        Assert.False(string.IsNullOrEmpty(responseValue.TicketId));
    }
}