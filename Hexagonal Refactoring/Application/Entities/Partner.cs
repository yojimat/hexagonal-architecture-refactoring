using System.Text.RegularExpressions;

namespace Hexagonal_Refactoring.Application.Entities;

public partial class Partner
{
    private Partner(PartnerId partnerId, string name, string cnpj, string email)
    {
        var cnpjIsValid = CnpjValidation().IsMatch(cnpj);
        if (!cnpjIsValid)
            throw new ArgumentException("Invalid CNPJ for partner", cnpj);

        var emailIsValid = EmailValidation().IsMatch(email);
        if (!emailIsValid)
            throw new ArgumentException("Invalid Email for partner", email);

        Name = name ?? throw new ArgumentException("Invalid Name for partner");
        Cnpj = cnpj;
        Email = email;
        PartnerId = partnerId;
    }

    public PartnerId PartnerId { get; private set; }
    public string Name { get; private set; }
    public string Cnpj { get; private set; }
    public string Email { get; private set; }

    public static Partner NewPartner(string name, string cpf, string email) =>
        new(PartnerId.NewId(), name, cpf, email);

    [GeneratedRegex(@"(\d{2}\.\d{3}\.\d{3}/\d{4}-\d{2})|(\d{14})$")]
    private static partial Regex CnpjValidation();

    [GeneratedRegex(@"^\S+@\S+\.\S+$")]
    private static partial Regex EmailValidation();
}