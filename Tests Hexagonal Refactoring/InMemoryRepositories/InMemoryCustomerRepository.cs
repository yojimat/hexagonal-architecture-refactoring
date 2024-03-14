using Hexagonal_Refactoring.Application.Domain;

namespace Tests_Hexagonal_Refactoring.InMemoryRepositories;

public class InMemoryCustomerRepository : ICustomerRepository
{
    private readonly Dictionary<string, Customer> _customers = [];
    private readonly Dictionary<string, Customer> _customersByCpf = [];
    private readonly Dictionary<string, Customer> _customersByEmail = [];

    public Customer? CustomerOfId(CustomerId id) => _customers.GetValueOrDefault(id.Id.ToString());
    public Customer? CustomerOfCpf(string cpf) => _customersByCpf.GetValueOrDefault(cpf);
    public Customer? CustomerOfEmail(string email) => _customersByEmail.GetValueOrDefault(email);

    public Customer Create(Customer customer)
    {
        _customers.Add(customer.CustomerId.ToString(), customer);
        _customersByCpf.Add(customer.Cpf, customer);
        _customersByEmail.Add(customer.Email.Value, customer);
        return customer;
    }

    public Customer Update(Customer customer)
    {
        _customers.Add(customer.CustomerId.ToString(), customer);
        _customersByCpf.Add(customer.Cpf, customer);
        _customersByEmail.Add(customer.Email.Value, customer);
        return customer;
    }

    // For tests purposes
    public void DeleteFromCpfDictionary(Customer customer) => _customersByCpf.Remove(customer.Cpf);
}