using Hexagonal_Refactoring.Application.UseCases.Partner;

namespace Integration_Tests_Hexagonal_Refactoring;

[Collection("Database tests")]
public class PartnerControllerTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private const string Url = "/api/partners";

    [Fact]
    public async Task PostPartner()
    {
        // Arrange
        var client = factory.CreateClient();
        NewPartnerDto partnerDto = new("John Doe", "11.545.127/0001-02", "johndoe@gmail.com");

        // Act
        var response = await client.PostAsJsonAsync(Url, partnerDto);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        var contentString = await response.Content.ReadAsStringAsync();
        var responseValue = JsonConvert.DeserializeObject<CreatePartnerUseCase.Output>(contentString);

        Assert.NotNull(responseValue);
        Assert.Equal(partnerDto.Email, responseValue.Email);
        Assert.Equal(partnerDto.Cnpj, responseValue.Cnpj);
        Assert.Equal(partnerDto.Name, responseValue.Name);
        Assert.False(string.IsNullOrEmpty(responseValue.Id));
    }

    [Fact]
    public async Task GetPartner()
    {
        // Arrange
        var client = factory.CreateClient();
        NewPartnerDto partnerDto = new("John Doe", "11.544.127/0001-02", "johndoe2@gmail.com");
        var createResponse = await client.PostAsJsonAsync(Url, partnerDto);
        createResponse.EnsureSuccessStatusCode(); // Status Code 200-299
        var contentString = await createResponse.Content.ReadAsStringAsync();
        var createValue = JsonConvert.DeserializeObject<CreatePartnerUseCase.Output>(contentString);

        //Act   
        Assert.NotNull(createValue);
        var response = await client.GetAsync($"{Url}/{createValue.Id}");
        response.EnsureSuccessStatusCode();
        contentString = await response.Content.ReadAsStringAsync();
        var value = JsonConvert.DeserializeObject<CreatePartnerUseCase.Output>(contentString);

        // Assert
        Assert.NotNull(value);
        Assert.Equal(partnerDto.Email, value.Email);
        Assert.Equal(partnerDto.Cnpj, value.Cnpj);
        Assert.Equal(partnerDto.Name, value.Name);
        Assert.Equal(createValue.Id, value.Id);
    }
}