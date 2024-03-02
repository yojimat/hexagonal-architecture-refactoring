using Hexagonal_Refactoring.Models;
using Hexagonal_Refactoring.Repositories;

namespace Hexagonal_Refactoring.Services;

public class CustomerService(ICustomerRepository repository) : ICustomerService
{
    //@Transactional
    public Customer Save(Customer customer)
    {
        return repository.Save(customer);
    }

    public Customer? FindById(long id)
    {
        return repository.FindById(id);
    }

    public Customer? FindByCpf(string? cpf)
    {
        return repository.FindByCpf(cpf);
    }

    public Customer? FindByEmail(string? email)
    {
        return repository.FindByEmail(email);
    }
}