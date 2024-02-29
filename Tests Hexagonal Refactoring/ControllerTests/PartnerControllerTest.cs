namespace Tests_Hexagonal_Refactoring.ControllerTests;

public class PartnerControllerTest
{
    private readonly Mock<IPartnerRepository> _partnerRepositoryMock = new();
    private readonly PartnerController _controller;
    private readonly Partner _expectedPartner;
    private readonly PartnerDto _partnerDto;

    public PartnerControllerTest()
    {
        _partnerDto = new PartnerDto();
        _partnerDto.SetCnpj("41536538000100");
        _partnerDto.SetEmail("john.doe@gmail.com");
        _partnerDto.SetName("John Doe");

        _controller = ControllerFactory();
        _expectedPartner = TestPartnerFactory(_partnerDto);
    }

    [Fact(DisplayName = "Should create a partner")]
    public void TestCreate()
    {
        // Arrange
        _partnerRepositoryMock
            .Setup(x => x.Save(It.Is<Partner>(cReceived => cReceived.GetId().Equals(_expectedPartner.GetId()))))
            .Returns(_expectedPartner);

        // Act
        var result = _controller.Create(_partnerDto);

        var exeResult = result as ObjectResult;

        Assert.NotNull(exeResult);
        var exeResultValue = exeResult.Value as PartnerDto;

        // Assert
        Assert.Equal(exeResult.StatusCode, StatusCodes.Status201Created);

        Assert.NotNull(exeResultValue);
        Assert.Equal(exeResultValue.GetEmail(), _partnerDto.GetEmail());
        Assert.Equal(exeResultValue.GetCnpj(), _partnerDto.GetCnpj());
        Assert.Equal(exeResultValue.GetName(), _partnerDto.GetName());
    }

    [Fact(DisplayName = "Should not register a partner with duplicated CNPJ")]
    public void TestCreateWithDuplicatedCnpjShouldFail()
    {
        // Arrange
        _partnerRepositoryMock
            .Setup(x => x.FindByCnpj(It.Is<string>(cReceived => cReceived.Equals(_expectedPartner.GetCnpj()))))
            .Returns(_expectedPartner);

        // Act
        var result = _controller.Create(_partnerDto);

        var exeResult = result as ObjectResult;
        Assert.NotNull(exeResult);
        Assert.Equal(exeResult.StatusCode, StatusCodes.Status422UnprocessableEntity);
        Assert.Equal(exeResult.Value, "Partner already exists.");
    }

    [Fact(DisplayName = "Should not register a partner with duplicated Email")]
    public void TestCreateWithDuplicatedEmailShouldFail()
    {
        // Arrange
        _partnerRepositoryMock
            .Setup(x => x.FindByEmail(It.Is<string>(cReceived => cReceived.Equals(_expectedPartner.GetEmail()))))
            .Returns(_expectedPartner);

        // Act
        var result = _controller.Create(_partnerDto);

        var exeResult = result as ObjectResult;
        Assert.NotNull(exeResult);
        Assert.Equal(exeResult.StatusCode, StatusCodes.Status422UnprocessableEntity);
        Assert.Equal(exeResult.Value, "Partner already exists.");
    }

    [Fact(DisplayName = "Should find a partner by id")]
    public void TestFindById()
    {
        // Arrange
        _partnerRepositoryMock
            .Setup(x => x.FindById(It.Is<long>(cReceived => cReceived.Equals(_expectedPartner.GetId()))))
            .Returns(_expectedPartner);

        // Act
        var result = _controller.GetPartner(_expectedPartner.GetId());

        var exeResult = result as ObjectResult;

        Assert.NotNull(exeResult);
        var exeResultValue = exeResult.Value as PartnerDto;

        // Assert
        Assert.Equal(exeResult.StatusCode, StatusCodes.Status200OK);

        Assert.NotNull(exeResultValue);
        Assert.Equal(exeResultValue.GetId(), _partnerDto.GetId());
        Assert.Equal(exeResultValue.GetEmail(), _partnerDto.GetEmail());
        Assert.Equal(exeResultValue.GetCnpj(), _partnerDto.GetCnpj());
        Assert.Equal(exeResultValue.GetName(), _partnerDto.GetName());
    }

    private static Partner TestPartnerFactory(PartnerDto partnerDto) => new(partnerDto.GetId(), partnerDto.GetName(),
        partnerDto.GetCnpj(), partnerDto.GetEmail());

    private PartnerController ControllerFactory()
    {
        var partnerService = new PartnerService(_partnerRepositoryMock.Object);
        var controller = new PartnerController(partnerService);
        return controller;
    }
}

