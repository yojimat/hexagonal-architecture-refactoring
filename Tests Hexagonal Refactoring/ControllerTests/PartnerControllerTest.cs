
namespace Tests_Hexagonal_Refactoring.ControllerTests;

public class PartnerControllerTest
{
    private readonly PartnerController _controller = ControllerFactory();
    private readonly NewPartnerDto _partnerDto = new("John Doe", "25.823.559/0001-42", "john.doe@gmail.com");

    [Fact(DisplayName = "Should create a partner")]
    public void TestCreate()
    {
        // Act
        var result = _controller.Create(_partnerDto);
        var exeResult = result as ObjectResult;

        Assert.NotNull(exeResult);
        var exeResultValue = exeResult.Value as CreatePartnerUseCase.Output;

        // Assert
        Assert.NotNull(exeResultValue);
        Assert.Equal(StatusCodes.Status201Created, exeResult.StatusCode);
        Assert.Equal(_partnerDto.Email,exeResultValue.Email );
        Assert.Equal(_partnerDto.Cnpj, exeResultValue.Cnpj);
        Assert.Equal(_partnerDto.Name, exeResultValue.Name);
    }

    [Fact(DisplayName = "Should not register a partner with duplicated CNPJ")]
    public void TestCreateWithDuplicatedCnpjShouldFail()
    {
        // Act  
        _controller.Create(_partnerDto);
        var secondPartnerDto = new NewPartnerDto("John Doe", "25.823.559/0001-42", "CNPJ is tested first");
        var secondResult = _controller.Create(secondPartnerDto);
        var exeResult = secondResult as ObjectResult;

        // Assert 
        Assert.NotNull(exeResult);
        Assert.Equal(StatusCodes.Status422UnprocessableEntity, exeResult.StatusCode);
        Assert.Equal("Partner CNPJ already in use", exeResult.Value);
    }

    [Fact(DisplayName = "Should not register a partner with duplicated Email")]
    public void TestCreateWithDuplicatedEmailShouldFail()
    {
        // Act
        _controller.Create(_partnerDto);
        var secondPartnerDto = new NewPartnerDto("John Doe", "25.823.559/0001-41", "john.doe@gmail.com");
        var secondResult = _controller.Create(secondPartnerDto);
        var exeResult = secondResult as ObjectResult;

        // Assert
        Assert.NotNull(exeResult);
        Assert.Equal(StatusCodes.Status422UnprocessableEntity, exeResult.StatusCode);
        Assert.Equal("Partner Email already in use", exeResult.Value);
    }

    [Fact(DisplayName = "Should find a partner by id")]
    public void TestFindById()
    {
        // Arrange
        var createResult = _controller.Create(_partnerDto);
        var exeCreateResult = createResult as ObjectResult;
        var exeCreateResultValue = exeCreateResult?.Value as CreatePartnerUseCase.Output;

        Assert.NotNull(exeCreateResultValue);

        // Act
        var result = _controller.GetPartner(exeCreateResultValue.Id);
        var exeGetResult = result as ObjectResult;

        Assert.NotNull(exeGetResult);

        var exeResultValue = exeGetResult.Value as GetPartnerByIdUseCase.Output;

        // Assert
        Assert.NotNull(exeResultValue);
        Assert.Equal(StatusCodes.Status200OK, exeGetResult.StatusCode);
        Assert.Equal(_partnerDto.Email, exeResultValue.Email);
        Assert.Equal(_partnerDto.Cnpj, exeResultValue.Cnpj);
        Assert.Equal(_partnerDto.Name, exeResultValue.Name);
    }

    private static PartnerController ControllerFactory()
    {
        var partnerRepository = new InMemoryPartnerRepository();
        CreatePartnerUseCase createPartnerUseCase = new(partnerRepository);
        GetPartnerByIdUseCase getPartnerByIdUseCase = new(partnerRepository);
        var controller = new PartnerController(createPartnerUseCase, getPartnerByIdUseCase);
        return controller;
    }
}