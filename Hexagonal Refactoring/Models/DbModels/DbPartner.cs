using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Hexagonal_Refactoring.Models.DbModels;

[Table("Partners")]
[PrimaryKey("EventId")]
public class DbPartner : Partner
{
    public string Id
    {
        get => GetId();
        init => SetId(value);
    }

    public string? Name
    {
        get => GetName();
        init => SetName(value ?? "");
    }

    public string? Cnpj
    {
        get => GetCnpj();
        init => SetCnpj(value);
    }

    public string? Email
    {
        get => GetEmail();
        init => SetEmail(value);
    }
}