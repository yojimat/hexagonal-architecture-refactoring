using Hexagonal_Refactoring.Application.Domain.Partner;

namespace Hexagonal_Refactoring.Application.Repositories;

public interface IPartnerRepository
{
    Partner? PartnerOfId(PartnerId id);
    Partner? PartnerOfCnpj(string cnpj);
    Partner? PartnerOfEmail(string email);
    Partner Create(Partner partner);
    Partner Update(Partner partner);
}