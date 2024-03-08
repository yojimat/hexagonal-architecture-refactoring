using Hexagonal_Refactoring.Models;
using Hexagonal_Refactoring.Repositories;

namespace Hexagonal_Refactoring.Services;

public class PartnerService(IPartnerRepository repository) : IPartnerService
{
    public Partner Save(Partner partner)
    {
        return repository.Save(partner);
    }

    public Partner? FindById(string id)
    {
        return repository.FindById(id);
    }

    public Partner? FindByCnpj(string cnpj)
    {
        return repository.FindByCnpj(cnpj);
    }

    public Partner? FindByEmail(string email)
    {
        return repository.FindByEmail(email);
    }
}