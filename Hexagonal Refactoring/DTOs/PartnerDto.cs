using Hexagonal_Refactoring.Models;

namespace Hexagonal_Refactoring.DTOs;

public class PartnerDto()
{
    private long _id;
    private string? _name;
    private string? _cnpj;
    private string? _email;
    public long Id { get => GetId(); set => SetId(value); }
    public string? Name { get => GetName(); set => SetName(value); }
    public string? Cnpj { get => GetCnpj(); set => SetCnpj(value); }
    public string? Email { get => GetEmail(); set => SetEmail(value); }

    public PartnerDto(long id) : this()
    {
        _id = id;
    }

    public PartnerDto(Partner partner) : this()
    {
        _id = partner.GetId();
        Name = partner.GetName();
        Cnpj = partner.GetCnpj();
        Email = partner.GetEmail();
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

