using System.ComponentModel.DataAnnotations;

namespace Tests_Hexagonal_Refactoring.ApplicationTests;

public class CreatePartnerUseCaseTest
{
    [Fact(DisplayName = "Should create a partner")]
    public void TestCreatePartner()
    {
        // Given
        const string expectedCnpj = "11.545.127/0001-02";
        const string expectedName = "John Doe";
        const string expectedEmail = "test@test.com";

        CreatePartnerUseCase.Input createInput = new(expectedCnpj, expectedEmail, expectedName);

        var repository = new InMemoryPartnerRepository();
        CreatePartnerUseCase useCase = new(repository);

        // When
        var output = useCase.Execute(createInput);

        // Then
        Assert.False(string.IsNullOrEmpty(output.Id));
        Assert.Equal(expectedCnpj, output.Cnpj);
        Assert.Equal(expectedName, output.Name);
        Assert.Equal(expectedEmail, output.Email);
    }

    [Fact(DisplayName = "Shouldn't create a partner with duplicated CNPJ")]
    public void TestCreateWithDuplicatedCnpjShouldFail()
    {
        // Given
        const string expectedCnpj = "11.545.127/0001-02";
        const string expectedName = "John Doe";
        const string expectedEmail = "test@test.com";
        const string expectedExceptionMessage = "Partner CNPJ already in use";

        var newPartner = Partner.NewPartner(expectedName, expectedCnpj, expectedEmail);

        var partnerRepository = new InMemoryPartnerRepository();
        partnerRepository.Create(newPartner);

        CreatePartnerUseCase.Input createInput = new(expectedCnpj, expectedEmail, expectedName);

        CreatePartnerUseCase useCase = new(partnerRepository);

        // Then
        var actualException = Assert.Throws<ValidationException>(() => useCase.Execute(createInput));

        Assert.NotNull(actualException);
        Assert.Equal(expectedExceptionMessage, actualException.Message);
    }

    [Fact(DisplayName = "Shouldn't create a partner with duplicated email")]
    public void TestCreateWithDuplicatedEmailShouldFail()
    {
        // Given
        const string expectedCnpj = "11.545.127/0001-02";
        const string expectedName = "John Doe";
        const string expectedEmail = "test@test.com";
        const string expectedExceptionMessage = "Partner Email already in use";

        var customer = Partner.NewPartner(expectedName, expectedCnpj, expectedEmail);

        var customerRepository = new InMemoryPartnerRepository();
        customerRepository.Create(customer);
        customerRepository.DeleteFromCnpjDictionary(customer);

        CreatePartnerUseCase.Input createInput = new(expectedCnpj, expectedEmail, expectedName);

        CreatePartnerUseCase useCase = new(customerRepository);

        // Then
        var actualException = Assert.Throws<ValidationException>(() => useCase.Execute(createInput));

        Assert.NotNull(actualException);
        Assert.Equal(expectedExceptionMessage, actualException.Message);
    }
}