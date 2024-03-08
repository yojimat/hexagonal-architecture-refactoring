using Hexagonal_Refactoring.Models;

namespace Hexagonal_Refactoring.Repositories;

public interface IPartnerRepository : ICrudRepository<Partner, string>
{
    Partner? FindByCnpj(string? cnpj);
    Partner? FindByEmail(string? email);
}