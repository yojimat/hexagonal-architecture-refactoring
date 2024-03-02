using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Hexagonal_Refactoring.Models.DbModels;

[Table("Partners")]
[PrimaryKey("Id")]
public class DbPartner : Partner
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id
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