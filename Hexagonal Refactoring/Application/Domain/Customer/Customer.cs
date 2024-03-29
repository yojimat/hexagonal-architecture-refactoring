﻿using System.Text.RegularExpressions;

namespace Hexagonal_Refactoring.Application.Domain.Customer;

public partial class Customer
{
    private Customer(CustomerId customerId, string name, string cpf, Email email)
    {
        var cpfIsValid = CpfValidation().IsMatch(cpf);
        if (!cpfIsValid)
            throw new ArgumentException("Invalid CPF for customer", cpf);

        Name = name ?? throw new ArgumentException("Invalid Name for customer");
        Cpf = cpf;
        Email = email;
        CustomerId = customerId;
    }

    public CustomerId CustomerId { get; private set; }
    public Email Email { get; private set; }
    public string Name { get; private set; }
    public string Cpf { get; private set; }

    public static Customer NewCustomer(string name, string cpf, string email) =>
        new(CustomerId.NewId(), name, cpf, new Email(email));

    public static Customer Restore(CustomerId customerId, string name, string cpf, string email) =>
        new(customerId, name, cpf, new Email(email));

    [GeneratedRegex(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$")]
    private static partial Regex CpfValidation();

}