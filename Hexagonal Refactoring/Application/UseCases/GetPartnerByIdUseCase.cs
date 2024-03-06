using Hexagonal_Refactoring.Application.Entities;
using Hexagonal_Refactoring.Application.Repositories;

namespace Hexagonal_Refactoring.Application.UseCases;

public class GetPartnerByIdUseCase(IPartnerRepository partnerRepository)
    : UseCase<GetPartnerByIdUseCase.Input, GetPartnerByIdUseCase.Output>
{
    public override Output? Execute(Input input)
    {
        var partner = partnerRepository.PartnerOfId(PartnerId.WithId(input.Id));
        var output = partner == null
            ? null
            : new Output(partner.PartnerId.ToString(), partner.Cnpj, partner.Email.Value,
                partner.Name);
        return output;
    }

    public record Input(string Id);

    public record Output(string Id, string Cnpj, string Email, string Name);
}