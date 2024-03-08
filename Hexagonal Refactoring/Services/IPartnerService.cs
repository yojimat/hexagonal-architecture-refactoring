using Hexagonal_Refactoring.Models;

namespace Hexagonal_Refactoring.Services;

public interface IPartnerService
{
    Partner? Save(Partner partner);
    Partner? FindById(string id);
    Partner? FindByCnpj(string cnpj);
    Partner? FindByEmail(string email);
}