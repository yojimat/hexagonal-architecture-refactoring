using System.Text.RegularExpressions;

namespace Hexagonal_Refactoring.Application.Entities;

public partial class Customer
{
    private Customer(CustomerId customerId, string name, string cpf, string email)
    {
        var cpfIsValid = CpfValidation().IsMatch(cpf);
        if (!cpfIsValid)
            throw new ArgumentException("Invalid CPF for customer", cpf);

        var emailIsValid = EmailValidation().IsMatch(email);
        if (!emailIsValid)
            throw new ArgumentException("Invalid Email for customer", email);

        Name = name;
        Cpf = cpf;
        Email = email;
        CustomerId = customerId;
    }

    public CustomerId CustomerId { get; private set; }
    public string Name { get; private set; }
    public string Cpf { get; private set; }
    public string Email { get; private set; }

    public static Customer NewCustomer(string name, string cpf, string email) =>
        new(CustomerId.NewId(), name, cpf, email);

    [GeneratedRegex(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$")]
    private static partial Regex CpfValidation();

    [GeneratedRegex(@"^\S+@\S+\.\S+$")]
    private static partial Regex EmailValidation();
}