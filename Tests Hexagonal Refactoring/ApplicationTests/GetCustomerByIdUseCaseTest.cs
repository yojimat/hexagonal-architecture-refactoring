using Hexagonal_Refactoring.Application.Domain;

namespace Tests_Hexagonal_Refactoring.ApplicationTests;

public class GetCustomerByIdUseCaseTest
{
    [Fact(DisplayName = "Should get a client by id")]
    public void TestGet()
    {
        // Arrange
        const string expectedCpf = "123.456.789-00";
        const string expectedName = "John Doe";
        const string expectedEmail = "test@test.com";

        var customer = Customer.NewCustomer(expectedName, expectedCpf, expectedEmail);

        var customerRepository = new InMemoryCustomerRepository();
        customerRepository.Create(customer);

        GetCustomerByIdUseCase.Input getInput = new(customer.CustomerId.ToString());

        GetCustomerByIdUseCase useCase = new(customerRepository);
        // Act
        var output = useCase.Execute(getInput);

        // Assert
        Assert.NotNull(output);
        Assert.Equal(expectedEmail, output.Email);
        Assert.Equal(expectedCpf, output.Cpf);
        Assert.Equal(expectedName, output.Name);
        Assert.Equal(customer.CustomerId.ToString(), output.Id);
    }

    [Fact(DisplayName = "Should return null when client not found")]
    public void TestByIdWithInvalidId()
    {
        // Arrange
        GetCustomerByIdUseCase.Input getInput = new(Guid.NewGuid().ToString());

        var customerRepository = new InMemoryCustomerRepository();

        GetCustomerByIdUseCase useCase = new(customerRepository);

        // Act
        var output = useCase.Execute(getInput);

        // Assert
        Assert.Null(output);
    }
}