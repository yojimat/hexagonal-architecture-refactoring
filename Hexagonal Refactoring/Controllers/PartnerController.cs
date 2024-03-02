using System.ComponentModel.DataAnnotations;
using Hexagonal_Refactoring.Application.UseCases;
using Hexagonal_Refactoring.DTOs;
using Hexagonal_Refactoring.Models;
using Hexagonal_Refactoring.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hexagonal_Refactoring.Controllers;

[ApiController]
[Route("api/partners")]
public class PartnerController(IPartnerService partnerService) : ControllerBase
{
    [HttpPost]
    public IActionResult Create([FromBody] PartnerDto dto)
    {
        try
        {
            var partnerCreateUseCase = new CreatePartnerUseCase(partnerService);
            var output =
                partnerCreateUseCase.Execute(new CreatePartnerUseCase.Input(dto.GetCnpj(), dto.GetEmail(),
                    dto.GetName()));

            return Created($"api/partners/{output.Id}",
                new PartnerDto(new Partner(output.Id, output.Name, output.Cnpj, output.Email)));
        }
        catch (ValidationException e)
        {
            return UnprocessableEntity(e.Message);
        }
    }

    [HttpGet("{id:long}")]
    public IActionResult GetPartner(long id)
    {
        var getPartnerByIdUseCase = new GetPartnerByIdUseCase(partnerService);
        var output = getPartnerByIdUseCase.Execute(new GetPartnerByIdUseCase.Input(id));
        return output == null
            ? NotFound()
            : Ok(new PartnerDto(new Partner(output.Id, output.Name, output.Cnpj, output.Email)));
    }
}