using System.ComponentModel.DataAnnotations;

namespace Tests_Hexagonal_Refactoring.ApplicationTests;

public class CreateCustomerUseCaseTest
{
    [Fact(DisplayName = "Should create a customer")]
    public void TestCreateCustomer()
    {
        // Given
        const string expectedCpf = "123.456.789-00";
        const string expectedName = "John Doe";
        const string expectedEmail = "test@test.com";

        CreateCustomerUseCase.Input createInput = new(expectedCpf, expectedEmail, expectedName);

        var repository = new InMemoryCustomerRepository();
        CreateCustomerUseCase useCase = new(repository);

        // When
        var output = useCase.Execute(createInput);

        // Then
        Assert.False(string.IsNullOrEmpty(output.Id));
        Assert.Equal(expectedCpf, output.Cpf);
        Assert.Equal(expectedName, output.Name);
        Assert.Equal(expectedEmail, output.Email);
    }

    [Fact(DisplayName = "Shouldn't create a client with duplicated CPF")]
    public void TestCreateWithDuplicatedCpfShouldFail()
    {
        // Given
        const string expectedCpf = "123.456.789-00";
        const string expectedName = "John Doe";
        const string expectedEmail = "test@test.com";
        const string expectedExceptionMessage = "Customer CPF already in use";

        var customer = Customer.NewCustomer(expectedName, expectedCpf, expectedEmail);

        var customerRepository = new InMemoryCustomerRepository();
        customerRepository.Create(customer);

        CreateCustomerUseCase.Input createInput = new(expectedCpf, expectedEmail, expectedName);

        CreateCustomerUseCase useCase = new(customerRepository);

        // Then
        var actualException = Assert.Throws<ValidationException>(() => useCase.Execute(createInput));

        Assert.NotNull(actualException);
        Assert.Equal(expectedExceptionMessage, actualException.Message);
    }

    [Fact(DisplayName = "Shouldn't create a client with duplicated email")]
    public void TestCreateWithDuplicatedEmailShouldFail()
    {
        // Given
        const string expectedCpf = "123.456.789-00";
        const string expectedName = "John Doe";
        const string expectedEmail = "test@test.com";
        const string expectedExceptionMessage = "Customer Email already in use";

        var customer = Customer.NewCustomer(expectedName, expectedCpf, expectedEmail);

        var customerRepository = new InMemoryCustomerRepository();
        customerRepository.Create(customer);
        customerRepository.DeleteFromCpfDictionary(customer);

        CreateCustomerUseCase.Input createInput = new(expectedCpf, expectedEmail, expectedName);

        CreateCustomerUseCase useCase = new(customerRepository);

        // Then
        var actualException = Assert.Throws<ValidationException>(() => useCase.Execute(createInput));

        Assert.NotNull(actualException);
        Assert.Equal(expectedExceptionMessage, actualException.Message);
    }
}