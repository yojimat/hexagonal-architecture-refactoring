using System.ComponentModel.DataAnnotations;
using Hexagonal_Refactoring.Application.UseCases;
using Hexagonal_Refactoring.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Hexagonal_Refactoring.Controllers;

[ApiController]
[Route("api/events")]
public class EventController(
    CreateEventUseCase createEventUseCase,
    SubscribeCustomerToEventUseCase subscribeCustomerToEventUseCase) : ControllerBase
{
    [HttpPost]
    public IActionResult Create([FromBody] NewEventDto dto)
    {
        if (dto.Partner is null) return BadRequest("Partner is required");

        try
        {
            var output = createEventUseCase.Execute(new CreateEventUseCase.Input(dto.Name ?? string.Empty,
                dto.Date ?? string.Empty, dto.TotalSpots,
                dto.Partner.GetId()));

            return Created($"/events/{output.Id}", dto);
        }
        catch (ValidationException ex)
        {
            Console.WriteLine(ex);
            return UnprocessableEntity(ex.Message);
        }
    }

    [HttpPost("{id:long}/subscribe")]
    public IActionResult Subscribe(long id, [FromBody] SubscribeDto dto)
    {
        try
        {
            subscribeCustomerToEventUseCase.Execute(new SubscribeCustomerToEventUseCase.Input(id, dto.CustomerId));

            return Ok();
        }
        catch (ValidationException ex)
        {
            Console.WriteLine(ex);
            return UnprocessableEntity(ex.Message);
        }
    }
}