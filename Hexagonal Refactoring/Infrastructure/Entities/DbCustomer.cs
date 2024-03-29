﻿using System.ComponentModel.DataAnnotations.Schema;
using Hexagonal_Refactoring.Application.Domain.Customer;
using Microsoft.EntityFrameworkCore;

namespace Hexagonal_Refactoring.Infrastructure.Entities;

[Table("Customers")]
[PrimaryKey("Id")]
public class DbCustomer(string name, string cpf, string email)  
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; init; }

    [Column(TypeName = "varchar(100)")]
    public string Name { get; init; } = name;

    [Column(TypeName = "varchar(100)")]
    public string Cpf { get; init; } = cpf;

    [Column(TypeName = "varchar(100)")]
    public string Email { get; init; } = email;

    public static DbCustomer FromCustomer(Customer customer) =>
       new(customer.Name, customer.Cpf, customer.Email.Value);

    public Customer ToCustomer() =>
        Customer.Restore(new CustomerId(Id), Name, Cpf, Email);
}