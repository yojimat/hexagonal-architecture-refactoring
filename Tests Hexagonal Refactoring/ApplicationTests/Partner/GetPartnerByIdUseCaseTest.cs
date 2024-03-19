using DomainPartner = Hexagonal_Refactoring.Application.Domain.Partner;
using Hexagonal_Refactoring.Application.UseCases.Partner;

namespace Tests_Hexagonal_Refactoring.ApplicationTests.Partner;

public class GetPartnerByIdUseCaseTest
{
    [Fact(DisplayName = "Should get a partner by id")]
    public void TestGet()
    {
        // Arrange
        const string expectedCnpj = "11.545.127/0001-02";
        const string expectedName = "John Doe";
        const string expectedEmail = "test@test.com";

        var partner = DomainPartner.Partner.NewPartner(expectedName, expectedCnpj, expectedEmail);

        var customerRepository = new InMemoryPartnerRepository();
        customerRepository.Create(partner);

        GetPartnerByIdUseCase.Input getInput = new(partner.PartnerId.ToString());

        GetPartnerByIdUseCase useCase = new(customerRepository);

        // Act
        var output = useCase.Execute(getInput);

        // Assert
        Assert.NotNull(output);
        Assert.Equal(expectedEmail, output.Email);
        Assert.Equal(expectedCnpj, output.Cnpj);
        Assert.Equal(expectedName, output.Name);
        Assert.Equal(partner.PartnerId.ToString(), output.Id);
    }

    [Fact(DisplayName = "Should return null when partner not found")]
    public void TestWithInvalidId()
    {
        // Arrange
        GetPartnerByIdUseCase.Input getInput = new(Guid.NewGuid().ToString());

        var partnerRepository = new InMemoryPartnerRepository();

        GetPartnerByIdUseCase useCase = new(partnerRepository);

        // Act
        var output = useCase.Execute(getInput);

        // Assert
        Assert.Null(output);
    }
}