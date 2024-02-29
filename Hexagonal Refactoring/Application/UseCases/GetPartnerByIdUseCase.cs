using Hexagonal_Refactoring.Services;

namespace Hexagonal_Refactoring.Application.UseCases;

public class GetPartnerByIdUseCase(IPartnerService partnerService)
    : UseCase<GetPartnerByIdUseCase.Input, GetPartnerByIdUseCase.Output>
{
    public record Input(long Id);
    public record Output(long Id, string Cnpj, string Email, string? Name);

    public override Output? Execute(Input getInput)
    {
        var partner = partnerService.FindById(getInput.Id);
        var output = partner == null
            ? null
            : new Output(partner.GetId(), partner.GetCnpj() ?? string.Empty, partner.GetEmail() ?? string.Empty,
                partner.GetName());
        return output;
    }
}
