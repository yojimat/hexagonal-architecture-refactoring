using Hexagonal_Refactoring.Application.UseCases;

namespace Tests_Hexagonal_Refactoring.ApplicationTests;
public class GetCustomerByIdUseCaseTest
{
    [Fact(DisplayName = "Should get a client by id")]
    public void TestGet()
    {
        // Arrange
        const long expectedId = 1;
        const string expectedCpf = "123.456.789-00";
        const string expectedName = "John Doe";
        const string expectedEmail = "test@test.com";

        var mockCustomer = new Customer(expectedId, expectedName, expectedCpf, expectedEmail);

        Mock<ICustomerRepository> customerRepositoryMock = new();
        customerRepositoryMock.Setup(x => x.FindById(It.Is<long>(idReceived =>
                idReceived.Equals(mockCustomer.GetId()))))
            .Returns(mockCustomer);

        var service = new CustomerService(customerRepositoryMock.Object);

        GetCustomerByIdUseCase.Input getInput = new(mockCustomer.GetId());

        // Act
        GetCustomerByIdUseCase useCase = new(service);
        var output = useCase.Execute(getInput);

        // Assert
        Assert.NotNull(output);
        Assert.Equal(output.Email, mockCustomer.GetEmail());
        Assert.Equal(output.Cpf, mockCustomer.GetCpf());
        Assert.Equal(output.Name, mockCustomer.GetName());
        Assert.Equal(output.Id, mockCustomer.GetId());
    }

    [Fact(DisplayName = "Should return null when client not found")]
    public void TestByIdWithInvalidId()
    {
        // Arrange
        const long expectedId = 1;

        Mock<ICustomerRepository> customerRepositoryMock = new();
        customerRepositoryMock.Setup(x => x.FindById(It.Is<long>(idReceived =>
                           idReceived.Equals(expectedId))))
            .Returns(null as Customer);

        var service = new CustomerService(customerRepositoryMock.Object);

        GetCustomerByIdUseCase.Input getInput = new(expectedId);

        // Act
        GetCustomerByIdUseCase useCase = new(service);
        var output = useCase.Execute(getInput);

        // Assert
        Assert.Null(output);
    }
}