namespace Hexagonal_Refactoring.Models;

public class Customer()
{
    private long _id;
    private string? _name;
    private string? _cpf;
    private string? _email;

    public Customer(long id, string? name, string? cpf, string? email) : this()
    {
        _id = id;
        _name = name;
        _cpf = cpf;
        _email = email;
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

    public override bool Equals(object? o)
    {
        if (this == o) return true;
        if (o == null || GetType() != o.GetType()) return false;
        var customer = (Customer)o;
        return Equals(_id, customer._id);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_id);
    }
}
