using System.Text.RegularExpressions;

namespace Hexagonal_Refactoring.Application.Domain.Partner;

public partial class Partner
{
    private Partner(PartnerId partnerId, string name, string cnpj, Email email)
    {
        var cnpjIsValid = CnpjValidation().IsMatch(cnpj);
        if (!cnpjIsValid)
            throw new ArgumentException("Invalid CNPJ for partner", cnpj);

        Name = name ?? throw new ArgumentException("Invalid Name for partner");
        Cnpj = cnpj;
        Email = email;
        PartnerId = partnerId;
    }

    public PartnerId PartnerId { get; private set; }
    public string Name { get; private set; }
    public string Cnpj { get; private set; }
    public Email Email { get; private set; }

    public static Partner NewPartner(string name, string cnpj, string email) =>
        new(PartnerId.NewId(), name, cnpj, new Email(email));

    public static Partner Restore(PartnerId partnerId, string name, string cnpj, Email email) => new(partnerId,name,cnpj,email);

    [GeneratedRegex(@"(\d{2}\.\d{3}\.\d{3}/\d{4}-\d{2})|(\d{14})$")]
    private static partial Regex CnpjValidation();

}