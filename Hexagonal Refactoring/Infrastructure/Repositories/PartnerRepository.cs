using Hexagonal_Refactoring.Application.Domain.Partner;
using Hexagonal_Refactoring.Application.Repositories;
using Hexagonal_Refactoring.Infrastructure.Contexts;
using Hexagonal_Refactoring.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hexagonal_Refactoring.Infrastructure.Repositories;

public class PartnerRepository(EventContext eventContext) : IPartnerRepository
{
    public Partner? PartnerOfId(PartnerId id) => eventContext.Partners.AsNoTracking()
        .FirstOrDefault(p => p.Id == Guid.Parse(id.ToString()))?.ToPartner();

    public Partner? PartnerOfCnpj(string cnpj) =>
        eventContext.Partners.FirstOrDefault(p => p.Cnpj == cnpj)?.ToPartner();

    public Partner? PartnerOfEmail(string email) =>
        eventContext.Partners.FirstOrDefault(p => p.Email == email)?.ToPartner();

    public Partner Create(Partner partner)
    {
        var newPartner = eventContext.Partners.Add(DbPartner.FromPartner(partner));
        eventContext.SaveChanges();
        return newPartner.Entity.ToPartner();
    }

    public Partner Update(Partner partner)
    {
        throw new NotImplementedException();
    }
}