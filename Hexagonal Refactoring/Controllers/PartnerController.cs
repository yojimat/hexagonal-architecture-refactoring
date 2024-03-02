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
    public IActionResult Create([FromBody] NewPartnerDto dto)
    {
        try
        {
            var output =
                createPartnerUseCase.Execute(new CreatePartnerUseCase.Input(dto.Cnpj, dto.Email,
                    dto.Name));

            return Created($"api/partners/{output.Id}", dto);
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
            : Ok(new Partner
                (output.Id, output.Name, output.Cnpj, output.Email));
    }
}