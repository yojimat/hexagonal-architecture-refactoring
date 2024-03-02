using System.ComponentModel.DataAnnotations;
using Hexagonal_Refactoring.Application.UseCases;

namespace Tests_Hexagonal_Refactoring.ApplicationTests;

public class CreatePartnerUseCaseTest
{
    [Fact(DisplayName = "Should create a partner")]
    public void TestCreatePartner()
    {
        // Given
        const string expectedCpnj = "123.456.789-00";
        const string expectedName = "John Doe";
        const string expectedEmail = "test@test.com";

        var mockPartner = new Partner(1, expectedName, expectedCpnj, expectedEmail);

        CreatePartnerUseCase.Input createInput = new(expectedCpnj, expectedEmail, expectedName);

        Mock<IPartnerRepository> partnerRepository = new();
        partnerRepository.Setup(x =>
                x.Save(It.Is<Partner>(c => c.GetCnpj() == expectedCpnj &&
                                           c.GetEmail() == expectedEmail &&
                                           c.GetName() == expectedName)))
            .Returns(mockPartner);

        var service = new PartnerService(partnerRepository.Object);

        // When
        CreatePartnerUseCase useCase = new(service);
        var output = useCase.Execute(createInput);

        // Then
        Assert.NotNull(output);
        Assert.Equal(1, output.Id);
        Assert.Equal(expectedCpnj, output.Cnpj);
        Assert.Equal(expectedName, output.Name);
        Assert.Equal(expectedEmail, output.Email);
    }

    [Fact(DisplayName = "Shouldn't create a partner with duplicated CNPJ")]
    public void TestCreateWithDuplicatedCnpjShouldFail()
    {
        // Given
        const string expectedCnpj = "123.456.789-00";
        const string expectedName = "John Doe";
        const string expectedEmail = "test@test.com";
        const string expectedExceptionMessage = "Partner already exists.";

        var mockPartner = new Partner(1, expectedName, expectedCnpj, expectedEmail);

        CreatePartnerUseCase.Input createInput = new(expectedCnpj, expectedEmail, expectedName);

        Mock<IPartnerRepository> partnerRepositoryMock = new();
        partnerRepositoryMock.Setup(x =>
                x.FindByCnpj(It.Is<string>(cpf => cpf.Equals(expectedCnpj))))
            .Returns(mockPartner);

        var service = new PartnerService(partnerRepositoryMock.Object);
        // When
        CreatePartnerUseCase useCase = new(service);

        // Then
        var actualException = Assert.Throws<ValidationException>(() => useCase.Execute(createInput));

        Assert.NotNull(actualException);
        Assert.Equal(expectedExceptionMessage, actualException.Message);
    }

    [Fact(DisplayName = "Shouldn't create a partner with duplicated email")]
    public void TestCreateWithDuplicatedEmailShouldFail()
    {
        // Given
        const string expectedCnpj = "123.456.789-00";
        const string expectedName = "John Doe";
        const string expectedEmail = "test@test.com";
        const string expectedExceptionMessage = "Partner already exists.";

        var mockPartner = new Partner(1, expectedName, expectedCnpj, expectedEmail);

        Mock<IPartnerRepository> partnerRepositoryMock = new();
        partnerRepositoryMock.Setup(x =>
                x.FindByEmail(It.Is<string>(email => email.Equals(expectedEmail))))
            .Returns(mockPartner);

        var service = new PartnerService(partnerRepositoryMock.Object);

        CreatePartnerUseCase.Input createInput = new(expectedCnpj, expectedEmail, expectedName);

        // When
        CreatePartnerUseCase useCase = new(service);

        // Then
        var actualException = Assert.Throws<ValidationException>(() => useCase.Execute(createInput));

        Assert.NotNull(actualException);
        Assert.Equal(expectedExceptionMessage, actualException.Message);
    }
}