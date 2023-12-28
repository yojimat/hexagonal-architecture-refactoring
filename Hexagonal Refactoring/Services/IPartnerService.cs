using Hexagonal_Refactoring.Models;

namespace Hexagonal_Refactoring.Services;

public interface IPartnerService
{
    Partner? Save(Partner partner);
    Partner? FindById(long id);
    Partner? FindByCnpj(string cnpj);
    Partner? FindByEmail(string email);
}
