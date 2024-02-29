using Hexagonal_Refactoring.Application.UseCases;

namespace Tests_Hexagonal_Refactoring.ApplicationTests;
public class GetPartnerByIdUseCaseTest
{
    [Fact(DisplayName = "Should get a partner by id")]
    public void TestGet()
    {
        // Arrange
        const long expectedId = 1;
        const string expectedCnpj = "123.456.789-00";
        const string expectedName = "John Doe";
        const string expectedEmail = "test@test.com";

        var mockPartner = new Partner(expectedId, expectedName, expectedCnpj, expectedEmail);

        Mock<IPartnerRepository> partnerRepositoryMock = new();
        partnerRepositoryMock.Setup(x => x.FindById(It.Is<long>(idReceived =>
                idReceived.Equals(mockPartner.GetId()))))
            .Returns(mockPartner);

        var service = new PartnerService(partnerRepositoryMock.Object);

        GetPartnerByIdUseCase.Input getInput = new(mockPartner.GetId());

        // Act
        GetPartnerByIdUseCase useCase = new(service);
        var output = useCase.Execute(getInput);

        // Assert
        Assert.NotNull(output);
        Assert.Equal(output.Email, mockPartner.GetEmail());
        Assert.Equal(output.Cnpj, mockPartner.GetCnpj());
        Assert.Equal(output.Name, mockPartner.GetName());
        Assert.Equal(output.Id, mockPartner.GetId());
    }

    [Fact(DisplayName = "Should return null when partner not found")]
    public void TestByIdWithInvalidId()
    {
        // Arrange
        const long expectedId = 1;

        Mock<IPartnerRepository> partnerRepositoryMock = new();
        partnerRepositoryMock.Setup(x => x.FindById(It.Is<long>(idReceived =>
                           idReceived.Equals(expectedId))))
            .Returns(null as Partner);

        var service = new PartnerService(partnerRepositoryMock.Object);

        GetPartnerByIdUseCase.Input getInput = new(expectedId);

        // Act
        GetPartnerByIdUseCase useCase = new(service);
        var output = useCase.Execute(getInput);

        // Assert
        Assert.Null(output);
    }
}