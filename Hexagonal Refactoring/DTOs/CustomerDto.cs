using Hexagonal_Refactoring.Models;

namespace Hexagonal_Refactoring.DTOs;

public class CustomerDto()
{
    private long _id;
    private string? _name;
    private string? _cpf;
    private string? _email;
    public long Id { get => GetId(); set => SetId(value); }
    public string Name { get => GetName(); set => SetName(value); }
    public string Cpf { get => GetCpf(); set => SetCpf(value); }
    public string? Email { get => GetEmail(); set => SetEmail(value); }

    public CustomerDto(Customer customer) : this()
    {
        _id = customer.GetId();
        _name = customer.GetName();
        _cpf = customer.GetCpf();
        _email = customer.GetEmail();
    }

    public long GetId()
    {
        return _id;
    }

    public void SetId(long id)
    {
        _id = id;
    }

    public string? GetName()
    {
        return _name;
    }

    public void SetName(string? name)
    {
        _name = name;
    }

    public string? GetCpf()
    {
        return _cpf;
    }

    public void SetCpf(string? cpf)
    {
        _cpf = cpf;
    }

    public string? GetEmail()
    {
        return _email;
    }

    public void SetEmail(string? email)
    {
        _email = email;
    }
}
