using AutoMapper;
using Hexagonal_Refactoring.Models;
using Hexagonal_Refactoring.Models.DbModels;
using Hexagonal_Refactoring.Repositories.Contexts;

namespace Hexagonal_Refactoring.Repositories;

public class CustomerRepository(EventContext eventContext, IMapper mapper) : ICustomerRepository
{
    public IEnumerable<Customer> GetAll()
    {
        throw new NotImplementedException();
    }

    public Customer? FindById(long id)
    {
        return eventContext.Customers.Find(id);
    }

    public Customer Save(Customer entity)
    {
        var dbCustomer = new DbCustomer();
        mapper.Map(entity, dbCustomer);
        var evEntityEntry = eventContext.Customers.Add(dbCustomer);
        eventContext.SaveChanges();
        return evEntityEntry.Entity;
    }

    public void DeleteAll()
    {
        throw new NotImplementedException();
    }

    public Customer? FindByCpf(string? cpf)
    {
        return eventContext.Customers.FirstOrDefault(p => p.Cpf == cpf);
    }

    public Customer? FindByEmail(string? email)
    {
        return eventContext.Customers.FirstOrDefault(p => p.Email == email);
    }
}