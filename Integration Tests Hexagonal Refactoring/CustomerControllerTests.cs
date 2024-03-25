using Hexagonal_Refactoring.Application.Domain.Customer;
using Hexagonal_Refactoring.Application.UseCases.Customer;
using Hexagonal_Refactoring.Infrastructure.Contexts;
using Microsoft.Extensions.DependencyInjection;

namespace Integration_Tests_Hexagonal_Refactoring;

[Collection("Database tests")]
public class CustomerControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly NewCustomerDto _customerDto;
    private readonly Customer _expectedCustomer;
    private const string Url = "/api/customers";

    public CustomerControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;

        _customerDto = new NewCustomerDto("John Doe", "123.456.789-01", "johndoe@gmail.com");
        _expectedCustomer = TestCustomerFactory(_customerDto);
    }

    [Fact]
    public async Task PostCustomer()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync(Url, _customerDto);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        var contentString = await response.Content.ReadAsStringAsync();
        var responseValue = JsonConvert.DeserializeObject<CreateCustomerUseCase.Output>(contentString);

        Assert.NotNull(responseValue);
        Assert.Equal(_expectedCustomer.Email.Value, responseValue.Email);
        Assert.Equal(_expectedCustomer.Cpf, responseValue.Cpf);
        Assert.Equal(_expectedCustomer.Name, responseValue.Name);
        Assert.False(string.IsNullOrEmpty(responseValue.Id));
    }

    [Fact]
    public async Task GetCustomer()
    {
        // Arrange
        var client = _factory.CreateClient();
        var customerDto = new NewCustomerDto("John Doe", "124.456.789-01", "johndoe2@gmail.com");
        var createResponse = await client.PostAsJsonAsync(Url, customerDto);
        createResponse.EnsureSuccessStatusCode();// Status Code 200-299
        var contentString = await createResponse.Content.ReadAsStringAsync();
        var createResponseValue = JsonConvert.DeserializeObject<CreateCustomerUseCase.Output>(contentString);

        // Act
        Assert.NotNull(createResponseValue);
        var response = await client.GetAsync($"{Url}/{createResponseValue.Id}");

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        contentString = await response.Content.ReadAsStringAsync();
        var responseValue = JsonConvert.DeserializeObject<GetCustomerByIdUseCase.Output>(contentString);

        Assert.NotNull(responseValue);
        Assert.Equal(customerDto.Email, responseValue.Email);
        Assert.Equal(customerDto.Cpf, responseValue.Cpf);
        Assert.Equal(customerDto.Name, responseValue.Name);
        Assert.Equal(createResponseValue.Id, responseValue.Id);
    }

    private static Customer TestCustomerFactory(NewCustomerDto customer) =>
        Customer.NewCustomer(customer.Name, customer.Cpf, customer.Email);
}