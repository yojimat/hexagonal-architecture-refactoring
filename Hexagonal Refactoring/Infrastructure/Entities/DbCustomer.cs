using System.ComponentModel.DataAnnotations.Schema;
using Hexagonal_Refactoring.Application.Domain.Customer;
using Microsoft.EntityFrameworkCore;

namespace Hexagonal_Refactoring.Infrastructure.Entities;

[Table("Customers")]
[PrimaryKey("CustomerId")]
public class DbCustomer
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; init; }

    public string Name { get; init; }
    public string Cpf { get; init; }
    public string Email { get; init; }

    public static DbCustomer FromCustomer(Customer customer) =>
        new()
        {
            Name = customer.Name,
            Cpf = customer.Cpf,
            Email = customer.Email.Value
        };

    public Customer ToCustomer() =>
        Customer.Restore(new CustomerId(Id), Name, Cpf, Email);
}