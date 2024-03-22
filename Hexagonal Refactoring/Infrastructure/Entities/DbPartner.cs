using System.ComponentModel.DataAnnotations.Schema;
using Hexagonal_Refactoring.Application.Domain;
using Hexagonal_Refactoring.Application.Domain.Partner;
using Microsoft.EntityFrameworkCore;

namespace Hexagonal_Refactoring.Infrastructure.Entities;

[Table("Partners")]
[PrimaryKey("Id")]
public class DbPartner(string name, string cnpj, string email)
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; init; }

    [Column(TypeName = "varchar(100)")]
    public string Name { get; init; } = name;

    [Column(TypeName = "varchar(100)")]
    public string Cnpj { get; init; } = cnpj;

    [Column(TypeName = "varchar(100)")]
    public string Email { get; init; } = email;

    public static DbPartner FromPartner(Partner partner) => new(partner.Name,partner.Cnpj,partner.Email.Value);

    public Partner ToPartner() =>
        Partner.Restore(new PartnerId(Id), Name, Cnpj, new Email(Email));
}