using Hexagonal_Refactoring.Models;

namespace Hexagonal_Refactoring.Repositories;

public interface ICustomerRepository : ICrudRepository<Customer, long>
{
    Customer? FindByCpf(string? cpf);
    Customer? FindByEmail(string? email);
}