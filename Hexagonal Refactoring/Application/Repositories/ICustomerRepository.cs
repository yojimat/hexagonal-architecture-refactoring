using Hexagonal_Refactoring.Application.Domain.Customer;

namespace Hexagonal_Refactoring.Application.Repositories;

public interface ICustomerRepository
{
    Customer? CustomerOfId(CustomerId id);
    Customer? CustomerOfCpf(string cpf);
    Customer? CustomerOfEmail(string email);
    Customer Create(Customer customer);
}