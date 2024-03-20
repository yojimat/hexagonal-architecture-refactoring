using Hexagonal_Refactoring.Application.Domain.Customer;
using Hexagonal_Refactoring.Application.UseCases.Customer;

namespace Integration_Tests_Hexagonal_Refactoring;

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

    private static Customer TestCustomerFactory(NewCustomerDto customer) =>
        Customer.NewCustomer(customer.Name, customer.Cpf, customer.Email);
}