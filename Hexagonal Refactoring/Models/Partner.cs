namespace Hexagonal_Refactoring.Models;

public class Partner()
{
    private string? _cnpj;
    private string? _email;
    private long _id;
    private string? _name;

    public Partner(long id, string? name, string? cnpj, string? email) : this()
    {
        _id = id;
        _name = name;
        _cnpj = cnpj;
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

    public void SetName(string name)
    {
        _name = name;
    }

    public string? GetCnpj()
    {
        return _cnpj;
    }

    public void SetCnpj(string? cnpj)
    {
        _cnpj = cnpj;
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