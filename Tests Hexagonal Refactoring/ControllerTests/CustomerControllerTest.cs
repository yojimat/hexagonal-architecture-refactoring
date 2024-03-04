using System.Net.Mime;
using Hexagonal_Refactoring.Application.UseCases;
using Hexagonal_Refactoring.Models;
using Hexagonal_Refactoring.Repositories;

namespace Tests_Hexagonal_Refactoring.ControllerTests;

public class CustomerControllerTest
{
    private readonly CustomerController _controller;
    private readonly NewCustomerDto _customerDto;
    private readonly Mock<ICustomerRepository> _customerRepositoryMock = new();
    private readonly Customer _expectedCustomer;

    public CustomerControllerTest()
    {
        _customerDto = new NewCustomerDto("John Doe", "123.456.789-01", "johndoe@gmail.com");

        _expectedCustomer = TestCustomerFactory(_customerDto);
        _controller = ServiceMockSave(_expectedCustomer);
    }

    [Fact(DisplayName = "Should create a client")]
    public void CreateClient()
    {
        // Act
        var result = _controller.Create(_customerDto);
        var exeResult = result as ObjectResult;

        Assert.NotNull(exeResult);
        var exeResultValue = exeResult.Value as CreateCustomerUseCase.Output;

        // Assert
        Assert.NotNull(exeResult);
        Assert.Equal(exeResult.StatusCode, StatusCodes.Status201Created);

        Assert.NotNull(exeResultValue);
        Assert.Equal(_expectedCustomer.GetEmail(), exeResultValue.Email);
        Assert.Equal(_expectedCustomer.GetCpf(), exeResultValue.Cpf);
        Assert.Equal(_expectedCustomer.GetName(), exeResultValue.Name);
        Assert.False(string.IsNullOrEmpty(_expectedCustomer.GetId().ToString()));
    }

    [Fact(DisplayName = "Shouldn't create a client with duplicated CPF")]
    public void TestCreateWithDuplicatedCpfShouldFail()
    {
        // Act
        // Create the first client
        var result = _controller.Create(_customerDto);
        var exeResult = result as ObjectResult;

        Assert.NotNull(exeResult);
        var exeResultValue = exeResult.Value as CreateCustomerUseCase.Output;


        // Create the second client with the same CPF
        var result2 = _controller.Create(_customerDto);
        var exeResult2 = result2 as ObjectResult;

        // Assert
        Assert.NotNull(exeResultValue);
        Assert.Equal(StatusCodes.Status201Created, exeResult.StatusCode);
        Assert.Equal(_expectedCustomer.GetEmail(), exeResultValue.Email);
        Assert.Equal(_expectedCustomer.GetCpf(), exeResultValue.Cpf);
        Assert.Equal(_expectedCustomer.GetName(), exeResultValue.Name);
        Assert.False(string.IsNullOrEmpty(_expectedCustomer.GetId().ToString()));

        Assert.NotNull(exeResult2);
        Assert.Equal(StatusCodes.Status422UnprocessableEntity, exeResult2.StatusCode);
        Assert.Equal("Customer CPF already in use", exeResult2.Value);
    }

    [Fact(DisplayName = "Shouldn't create a client with duplicated email")]
    public void TestCreateWithDuplicatedEmailShouldFail()
    {
        // Act
        // Create the first client
        var result = _controller.Create(_customerDto);
        var exeResult = result as ObjectResult;

        Assert.NotNull(exeResult);
        var exeResultValue = exeResult.Value as CreateCustomerUseCase.Output;

        // Create the second client with the same Email

        var customerDto2 = new NewCustomerDto("John Doe", "223.456.789-01", "johndoe@gmail.com");
        var result2 = _controller.Create(customerDto2);
        var exeResult2 = result2 as ObjectResult;

        // Assert
        Assert.NotNull(exeResultValue);
        Assert.Equal(StatusCodes.Status201Created, exeResult.StatusCode);
        Assert.Equal(_expectedCustomer.GetEmail(), exeResultValue.Email);
        Assert.Equal(_expectedCustomer.GetCpf(), exeResultValue.Cpf);
        Assert.Equal(_expectedCustomer.GetName(), exeResultValue.Name);
        Assert.False(string.IsNullOrEmpty(_expectedCustomer.GetId().ToString()));

        Assert.NotNull(exeResult2);
        Assert.Equal(StatusCodes.Status422UnprocessableEntity, exeResult2.StatusCode);
        Assert.Equal("Customer Email already in use", exeResult2.Value);
    }

    [Fact(DisplayName = "Should get a client by id")]
    public void TestGet()
    {
        // Arrange
        _customerRepositoryMock.Setup(x =>
                x.FindById(It.Is<long>(idReceived =>
                    idReceived.Equals(_expectedCustomer.GetId()))))
            .Returns(_expectedCustomer);

        // Act
        var result = _controller.Create(_customerDto);
        var exeResult = result as ObjectResult;
        var exeResultValue = exeResult?.Value as CreateCustomerUseCase.Output;

        Assert.NotNull(exeResultValue);

        var getResult = _controller.GetCustomer(exeResultValue.Id);
        var getExeResult = getResult as ObjectResult;

        // Assert 
        Assert.NotNull(getExeResult);
        Assert.Equal(StatusCodes.Status200OK, getExeResult.StatusCode);
        Assert.Equal(_expectedCustomer.GetEmail(), exeResultValue.Email);
        Assert.Equal(_expectedCustomer.GetCpf(), exeResultValue.Cpf);
        Assert.Equal(_expectedCustomer.GetName(), exeResultValue.Name);
        Assert.False(string.IsNullOrEmpty(_expectedCustomer.GetId().ToString()));
    }

    private CustomerController ServiceMockSave(Customer expectedCustomer)
    {
        _customerRepositoryMock.Setup(x =>
                x.Save(It.Is<Customer>(cReceived => cReceived.Equals(expectedCustomer))))
            .Returns(expectedCustomer);

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers.ContentType = MediaTypeNames.Application.Json;

        var controllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        var customerRepository = new InMemoryCustomerRepository();

        var createCustomerUseCase = new CreateCustomerUseCase(customerRepository);
        var getCustomerByIdUseCase = new GetCustomerByIdUseCase(customerRepository);

        var controller = new CustomerController(createCustomerUseCase, getCustomerByIdUseCase)
        {
            ControllerContext = controllerContext
        };

        return controller;
    }

    private static Customer TestCustomerFactory(NewCustomerDto customer)
    {
        return new Customer(0, customer.Name, customer.Cpf,
            customer.Email);
    }
}