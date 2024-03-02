using Hexagonal_Refactoring.Application.UseCases;

namespace Tests_Hexagonal_Refactoring.ControllerTests;

public class PartnerControllerTest
{
    private readonly PartnerController _controller;
    private readonly Partner _expectedPartner;
    private readonly NewPartnerDto _partnerDto;
    private readonly Mock<IPartnerRepository> _partnerRepositoryMock = new();

    public PartnerControllerTest()
    {
        _partnerDto = new NewPartnerDto("John Doe", "41536538000100", "john.doe@gmail.com");

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
        var exeResultValue = exeResult.Value as NewPartnerDto;

        // Assert
        Assert.Equal(exeResult.StatusCode, StatusCodes.Status201Created);

        Assert.NotNull(exeResultValue);
        Assert.Equal(exeResultValue.Email, _partnerDto.Email);
        Assert.Equal(exeResultValue.Cnpj, _partnerDto.Cnpj);
        Assert.Equal(exeResultValue.Name, _partnerDto.Name);
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
        var exeResultValue = exeResult.Value as Partner;

        // Assert
        Assert.Equal(exeResult.StatusCode, StatusCodes.Status200OK);

        Assert.NotNull(exeResultValue);
        Assert.Equal(exeResultValue.GetId(), _expectedPartner.GetId());
        Assert.Equal(exeResultValue.GetEmail(), _expectedPartner.GetEmail());
        Assert.Equal(exeResultValue.GetCnpj(), _expectedPartner.GetCnpj());
        Assert.Equal(exeResultValue.GetName(), _expectedPartner.GetName());
    }

    private static Partner TestPartnerFactory(NewPartnerDto partnerDto)
    {
        return new Partner(0, partnerDto.Name,
            partnerDto.Cnpj, partnerDto.Email);
    }

    private PartnerController ControllerFactory()
    {
        var partnerService = new PartnerService(_partnerRepositoryMock.Object);
        CreatePartnerUseCase createPartnerUseCase = new(partnerService);
        GetPartnerByIdUseCase getPartnerByIdUseCase = new(partnerService);
        var controller = new PartnerController(createPartnerUseCase, getPartnerByIdUseCase);
        return controller;
    }
}