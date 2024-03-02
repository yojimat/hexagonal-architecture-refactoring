using System.ComponentModel.DataAnnotations;
using Hexagonal_Refactoring.Application.UseCases;
using Hexagonal_Refactoring.DTOs;
using Hexagonal_Refactoring.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hexagonal_Refactoring.Controllers;

[ApiController]
[Route("api/partners")]
public class PartnerController(CreatePartnerUseCase createPartnerUseCase, GetPartnerByIdUseCase getPartnerByIdUseCase)
    : ControllerBase
{
    [HttpPost]
    public IActionResult Create([FromBody] PartnerDto dto)
    {
        try
        {
            var output =
                createPartnerUseCase.Execute(new CreatePartnerUseCase.Input(dto.GetCnpj(), dto.GetEmail(),
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
        var output = getPartnerByIdUseCase.Execute(new GetPartnerByIdUseCase.Input(id));
        return output == null
            ? NotFound()
            : Ok(new PartnerDto(new Partner(output.Id, output.Name, output.Cnpj, output.Email)));
    }
}