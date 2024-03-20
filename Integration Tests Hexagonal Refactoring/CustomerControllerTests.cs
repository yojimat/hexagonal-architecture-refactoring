using System.Net.Mime;
using Hexagonal_Refactoring.Application.Domain.Customer;
using Hexagonal_Refactoring.Application.UseCases.Customer;

namespace Integration_Tests_Hexagonal_Refactoring;

public class CustomerControllerTests
{
    private readonly CustomerController _controller;
    private readonly NewCustomerDto _customerDto;
    private readonly Customer _expectedCustomer;

    public CustomerControllerTests()
    {
        _customerDto = new NewCustomerDto("John Doe", "123.456.789-01", "johndoe@gmail.com");
        _expectedCustomer = TestCustomerFactory(_customerDto);
        _controller = ServiceMockSave();
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
        Assert.Equal(_expectedCustomer.Email.Value, exeResultValue.Email);
        Assert.Equal(_expectedCustomer.Cpf, exeResultValue.Cpf);
        Assert.Equal(_expectedCustomer.Name, exeResultValue.Name);
        Assert.False(string.IsNullOrEmpty(_expectedCustomer.CustomerId.ToString()));
    }


    [Fact(DisplayName = "Should get a client by id")]
    public void TestGet()
    {
        // Act
        var result = _controller.Create(_customerDto);
        var exeResult = result as ObjectResult;
        var exeResultValue = exeResult?.Value as CreateCustomerUseCase.Output;

        Assert.NotNull(exeResultValue);

        var getResult = _controller.GetCustomer(exeResultValue.Id);

        var getExeResult = getResult as ObjectResult;

        Assert.NotNull(getExeResult);

        var getExeResultValue = getExeResult.Value as GetCustomerByIdUseCase.Output;

        // Assert 
        Assert.NotNull(getExeResultValue);
        Assert.Equal(StatusCodes.Status200OK, getExeResult.StatusCode);
        Assert.Equal(_expectedCustomer.Email.Value, getExeResultValue.Email);
        Assert.Equal(_expectedCustomer.Cpf, getExeResultValue.Cpf);
        Assert.Equal(_expectedCustomer.Name, getExeResultValue.Name);
    }

    private static CustomerController ServiceMockSave()
    {
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

    private static Customer TestCustomerFactory(NewCustomerDto customer) =>
        Customer.NewCustomer(customer.Name, customer.Cpf, customer.Email);
}