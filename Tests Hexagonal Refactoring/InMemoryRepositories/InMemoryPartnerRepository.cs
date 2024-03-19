using Hexagonal_Refactoring.Application.Domain.Partner;

namespace Tests_Hexagonal_Refactoring.InMemoryRepositories;

public class InMemoryPartnerRepository : IPartnerRepository
{
    private readonly Dictionary<string, Partner> _customers = [];
    private readonly Dictionary<string, Partner> _customersByCnpj = [];
    private readonly Dictionary<string, Partner> _customersByEmail = [];

    public Partner? PartnerOfId(PartnerId id) => _customers.GetValueOrDefault(id.Id.ToString());
    public Partner? PartnerOfCnpj(string cnpj) => _customersByCnpj.GetValueOrDefault(cnpj);
    public Partner? PartnerOfEmail(string email) => _customersByEmail.GetValueOrDefault(email);

    public Partner Create(Partner customer)
    {
        _customers.Add(customer.PartnerId.ToString(), customer);
        _customersByCnpj.Add(customer.Cnpj, customer);
        _customersByEmail.Add(customer.Email.Value, customer);
        return customer;
    }

    public Partner Update(Partner customer)
    {
        _customers.Add(customer.PartnerId.ToString(), customer);
        _customersByCnpj.Add(customer.Cnpj, customer);
        _customersByEmail.Add(customer.Email.Value, customer);
        return customer;
    }

    // For tests purposes
    public void DeleteFromCnpjDictionary(Partner customer) => _customersByCnpj.Remove(customer.Cnpj);
}