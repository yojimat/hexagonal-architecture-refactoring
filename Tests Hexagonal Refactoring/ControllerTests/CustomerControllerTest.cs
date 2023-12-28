using System.Net.Mime;
using Hexagonal_Refactoring.Application.UseCases;

namespace Tests_Hexagonal_Refactoring.ControllerTests; public class CustomerControllerTest
{
    private readonly Mock<ICustomerRepository> _customerRepositoryMock = new();
    private readonly CustomerController _controller;
    private readonly CustomerDto _customerDto;
    private readonly Customer _expectedCustomer;

    public CustomerControllerTest()
    {
        _customerDto = new CustomerDto();
        _customerDto.SetCpf("12345678901");
        _customerDto.SetEmail("john.doe@gmail.com");
        _customerDto.SetName("John Doe");

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
        Assert.NotNull(exeResult); Assert.Equal(exeResult.StatusCode, StatusCodes.Status201Created);

        Assert.NotNull(exeResultValue);
        Assert.Equal(exeResultValue.Email, _expectedCustomer.GetEmail());
        Assert.Equal(exeResultValue.Cpf, _expectedCustomer.GetCpf());
        Assert.Equal(exeResultValue.Name, _expectedCustomer.GetName());
        Assert.Equal(exeResultValue.Id, _expectedCustomer.GetId());
    }

    [Fact(DisplayName = "Shouldn't create a client with duplicated CPF")]
    public void TestCreateWithDuplicatedEmailShouldFail()
    {
        // Act
        // Create the first client
        var result = _controller.Create(_customerDto);
        var exeResult = result as ObjectResult;

        Assert.NotNull(exeResult);
        var exeResultValue = exeResult.Value as CreateCustomerUseCase.Output;

        _customerDto.SetEmail("john2@gmail.com");
        _customerRepositoryMock.Setup(x =>
                           x.FindByCpf(It.Is<string>(cpfReceived => cpfReceived.Equals(_customerDto.GetCpf()))))
            .Returns(_expectedCustomer);

        // Create the second client with the same CPF
        var result2 = _controller.Create(_customerDto);
        var exeResult2 = result2 as ObjectResult;

        // Assert
        Assert.NotNull(exeResultValue);
        Assert.Equal(exeResult.StatusCode, StatusCodes.Status201Created);
        Assert.Equal(exeResultValue.Email, _expectedCustomer.GetEmail());
        Assert.Equal(exeResultValue.Cpf, _expectedCustomer.GetCpf());
        Assert.Equal(exeResultValue.Name, _expectedCustomer.GetName());
        Assert.Equal(exeResultValue.Id, _expectedCustomer.GetId());

        Assert.NotNull(exeResult2);
        Assert.Equal(exeResult2.StatusCode, StatusCodes.Status422UnprocessableEntity);
        Assert.Equal(exeResult2.Value, "Customer already exists.");
    }

    [Fact(DisplayName = "Shouldn't create a client with duplicated email")]
    public void TestCreateWithDuplicatedCpfShouldFail()
    {
        // Act
        // Create the first client
        var result = _controller.Create(_customerDto);
        var exeResult = result as ObjectResult;

        Assert.NotNull(exeResult);
        var exeResultValue = exeResult.Value as CreateCustomerUseCase.Output;

        _customerDto.SetCpf("22345678901");
        _customerRepositoryMock.Setup(x =>
                           x.FindByEmail(It.Is<string>(emailReceived => emailReceived.Equals(_customerDto.GetEmail()))))
            .Returns(_expectedCustomer);

        // Create the second client with the same CPF
        var result2 = _controller.Create(_customerDto);
        var exeResult2 = result2 as ObjectResult;

        // Assert
        Assert.NotNull(exeResultValue);
        Assert.Equal(exeResult.StatusCode, StatusCodes.Status201Created);
        Assert.Equal(exeResultValue.Email, _expectedCustomer.GetEmail());
        Assert.Equal(exeResultValue.Cpf, _expectedCustomer.GetCpf());
        Assert.Equal(exeResultValue.Name, _expectedCustomer.GetName());
        Assert.Equal(exeResultValue.Id, _expectedCustomer.GetId());

        Assert.NotNull(exeResult2);
        Assert.Equal(exeResult2.StatusCode, StatusCodes.Status422UnprocessableEntity);
        Assert.Equal(exeResult2.Value, "Customer already exists.");
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
        Assert.Equal(getExeResult.StatusCode, StatusCodes.Status200OK);
        Assert.Equal(exeResultValue.Email, _expectedCustomer.GetEmail());
        Assert.Equal(exeResultValue.Cpf, _expectedCustomer.GetCpf());
        Assert.Equal(exeResultValue.Name, _expectedCustomer.GetName());
        Assert.Equal(exeResultValue.Id, _expectedCustomer.GetId());
    }

    private CustomerController ServiceMockSave(Customer expectedCustomer)
    {
        _customerRepositoryMock.Setup(x =>
                x.Save(It.Is<Customer>(cReceived => cReceived.Equals(expectedCustomer))))
            .Returns(expectedCustomer);

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers.ContentType = MediaTypeNames.Application.Json;

        var controllerContext = new ControllerContext()
        {
            HttpContext = httpContext,
        };

        var service = new CustomerService(_customerRepositoryMock.Object);

        var controller = new CustomerController(service)
        {
            ControllerContext = controllerContext,
        };
        return controller;
    }

    private static Customer TestCustomerFactory(CustomerDto customer)
    {
        return new Customer(customer.GetId(), customer.GetCpf(), customer.GetName(),
            customer.GetEmail());
    }
}
