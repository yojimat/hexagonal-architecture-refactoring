using System.ComponentModel.DataAnnotations;
using Hexagonal_Refactoring.Models;
using Hexagonal_Refactoring.Services;

namespace Hexagonal_Refactoring.Application.UseCases;

public class CreatePartnerUseCase(IPartnerService partnerService)
    : UseCase<CreatePartnerUseCase.Input, CreatePartnerUseCase.Output>
{
    public record Input(string Cnpj, string Email, string Name);
    public record Output(long Id, string Cnpj, string Email, string Name);

    public override Output Execute(Input input)
    {
        if (partnerService.FindByCnpj(input.Cnpj) != null)
            throw new ValidationException("Partner already exists.");

        if (partnerService.FindByEmail(input.Email) != null)
            throw new ValidationException("Partner already exists.");

        var partner = new Partner();
        partner.SetName(input.Name);
        partner.SetCnpj(input.Cnpj);
        partner.SetEmail(input.Email);

        partner = partnerService.Save(partner);

        return new Output(partner.GetId(), partner.GetCnpj(), partner.GetEmail(), partner.GetName());
    }
}
