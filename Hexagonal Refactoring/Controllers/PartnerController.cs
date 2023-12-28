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
        if (partnerService.FindByCnpj(dto.GetCnpj()!) is not null)
        {
            return UnprocessableEntity("Partner already exists");
        }

        if (partnerService.FindByEmail(dto.GetEmail()!) is not null)
        {
            return UnprocessableEntity("Partner already exists");
        }

        var partner = new Partner();
        partner.SetName(dto.GetName()!);
        partner.SetCnpj(dto.GetCnpj()!);
        partner.SetEmail(dto.GetEmail()!);

        partner = partnerService.Save(partner);

        return Created($"api/partners/{partner!.GetId()}", new PartnerDto(partner));
    }

    [HttpGet("{id:long}")]
    public IActionResult GetPartner(long id)
    {
        var partner = partnerService.FindById(id);
        if (partner is null)
        {
            return NotFound();
        }

        return Ok(new PartnerDto(partner));
    }
}

