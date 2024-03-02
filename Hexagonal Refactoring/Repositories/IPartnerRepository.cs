using Hexagonal_Refactoring.Models;

namespace Hexagonal_Refactoring.Repositories;

public interface IPartnerRepository : ICrudRepository<Partner, long>
{
    Partner? FindByCnpj(string? cnpj);
    Partner? FindByEmail(string? email);
}