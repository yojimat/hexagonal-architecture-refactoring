using Hexagonal_Refactoring.Models;

namespace Hexagonal_Refactoring.Services;

public interface ICustomerService
{
    Customer Save(Customer customer);
    Customer? FindById(long id);
    Customer? FindByCpf(string? cpf);
    Customer? FindByEmail(string? email);
}