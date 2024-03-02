using AutoMapper;
using Hexagonal_Refactoring.Models;
using Hexagonal_Refactoring.Models.DbModels;
using Hexagonal_Refactoring.Repositories.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Hexagonal_Refactoring.Repositories;

public class PartnerRepository(EventContext eventContext, IMapper mapper) : IPartnerRepository
{
    public IEnumerable<Partner> GetAll()
    {
        throw new NotImplementedException();
    }

    public Partner? FindById(long id)
    {
        return eventContext.Partners.AsNoTracking().FirstOrDefault(p => p.Id == id);
    }

    public Partner Save(Partner entity)
    {
        var dbPartner = new DbPartner();
        mapper.Map(entity, dbPartner);
        var evnt = eventContext.Partners.Add(dbPartner);
        eventContext.SaveChanges();
        return evnt.Entity;
    }

    public void DeleteAll()
    {
        throw new NotImplementedException();
    }

    public Partner? FindByCnpj(string? cnpj)
    {
        return eventContext.Partners.FirstOrDefault(p => p.Cnpj == cnpj);
    }

    public Partner? FindByEmail(string? email)
    {
        return eventContext.Partners.FirstOrDefault(p => p.Email == email);
    }
}