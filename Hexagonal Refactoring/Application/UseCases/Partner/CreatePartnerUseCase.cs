﻿using System.ComponentModel.DataAnnotations;
using DomainPartner = Hexagonal_Refactoring.Application.Domain.Partner;
using Hexagonal_Refactoring.Application.Repositories;

namespace Hexagonal_Refactoring.Application.UseCases.Partner;

public class CreatePartnerUseCase(IPartnerRepository partnerRepository)
    : UseCase<CreatePartnerUseCase.Input, CreatePartnerUseCase.Output>
{
    public override Output Execute(Input input)
    {
        if (partnerRepository.PartnerOfCnpj(input.Cnpj) != null)
            throw new ValidationException("Partner CNPJ already in use");

        if (partnerRepository.PartnerOfEmail(input.Email) != null)
            throw new ValidationException("Partner Email already in use");

        var partner = partnerRepository.Create(DomainPartner.Partner.NewPartner(input.Name, input.Cnpj, input.Email));

        return new Output(partner.PartnerId.ToString(), partner.Cnpj, partner.Email.Value, partner.Name);
    }

    public record Input(string Cnpj, string Email, string Name);

    public record Output(string Id, string Cnpj, string Email, string Name);
}