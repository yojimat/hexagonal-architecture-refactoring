using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Hexagonal_Refactoring.Models.DbModels;

[Table("Customers")]
[PrimaryKey("EventId")]
public class DbCustomer : Customer
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

    public string? Cpf
    {
        get => GetCpf();
        init => SetCpf(value);
    }

    public string? Email
    {
        get => GetEmail();
        init => SetEmail(value);
    }
}