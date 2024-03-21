using Hexagonal_Refactoring.Application.Domain.Customer;
using Hexagonal_Refactoring.Application.Repositories;
using Hexagonal_Refactoring.Infrastructure.Contexts;
using Hexagonal_Refactoring.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hexagonal_Refactoring.Infrastructure.Repositories;

public class CustomerRepository(EventContext eventContext) : ICustomerRepository
{
    public Customer? CustomerOfId(CustomerId id) => eventContext.Customers.AsNoTracking().SingleOrDefault(c => c.Id == id.Id)?.ToCustomer();

    public Customer? CustomerOfCpf(string cpf) => eventContext.Customers.AsNoTracking().FirstOrDefault(p => p.Cpf == cpf)?.ToCustomer();

    public Customer? CustomerOfEmail(string email) => eventContext.Customers.FirstOrDefault(p => p.Email == email)?.ToCustomer();

    public Customer Create(Customer customer)
    {
        var dbCustomer = DbCustomer.FromCustomer(customer);
        var evEntityEntry = eventContext.Customers.Add(dbCustomer);
        eventContext.SaveChanges();
        return evEntityEntry.Entity.ToCustomer();
    }

    public Customer Update(Customer customer)
    {
        throw new NotImplementedException();
    }
}