using System.ComponentModel.DataAnnotations;
using Hexagonal_Refactoring.Application.UseCases;

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

        var mockCustomer = new Customer(1, expectedName, expectedCpf, expectedEmail);

        CreateCustomerUseCase.Input createInput = new(expectedCpf, expectedEmail, expectedName);

        Mock<ICustomerRepository> customerRepository = new();
        customerRepository.Setup(x =>
                x.Save(It.Is<Customer>(c => c.GetCpf() == expectedCpf &&
                                            c.GetEmail() == expectedEmail &&
                                            c.GetName() == expectedName)))
            .Returns(mockCustomer);

        var service = new CustomerService(customerRepository.Object);

        // When
        CreateCustomerUseCase useCase = new(service);
        var output = useCase.Execute(createInput);

        // Then
        Assert.NotNull(output);
        Assert.Equal(1, output.Id);
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
        const string expectedExceptionMessage = "Customer already exists.";

        var mockCustomer = new Customer(1, expectedName, expectedCpf, expectedEmail);

        CreateCustomerUseCase.Input createInput = new(expectedCpf, expectedEmail, expectedName);

        Mock<ICustomerRepository> customerRepositoryMock = new();
        customerRepositoryMock.Setup(x =>
                x.FindByCpf(It.Is<string>(cpf => cpf.Equals(expectedCpf))))
            .Returns(mockCustomer);


        var service = new CustomerService(customerRepositoryMock.Object);

        // When
        CreateCustomerUseCase useCase = new(service);

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
        const string expectedExceptionMessage = "Customer already exists.";

        var mockCustomer = new Customer(1, expectedName, expectedCpf, expectedEmail);

        Mock<ICustomerRepository> customerRepositoryMock = new();
        customerRepositoryMock.Setup(x =>
                x.FindByEmail(It.Is<string>(email => email.Equals(expectedEmail))))
            .Returns(mockCustomer);

        var service = new CustomerService(customerRepositoryMock.Object);

        CreateCustomerUseCase.Input createInput = new(expectedCpf, expectedEmail, expectedName);

        // When
        CreateCustomerUseCase useCase = new(service);

        // Then
        var actualException = Assert.Throws<ValidationException>(() => useCase.Execute(createInput));

        Assert.NotNull(actualException);
        Assert.Equal(expectedExceptionMessage, actualException.Message);
    }
}