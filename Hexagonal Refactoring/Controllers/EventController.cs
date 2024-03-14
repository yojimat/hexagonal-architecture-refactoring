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
        if (string.IsNullOrEmpty(dto.PartnerId)) return BadRequest("PartnerId is required");

        try
        {
            var output = createEventUseCase.Execute(new CreateEventUseCase.Input(dto.Name ?? string.Empty,
                dto.Date ?? string.Empty, dto.TotalSpots,
                dto.PartnerId));

            return Created($"/events/{output.EventId}", output);
        }
        catch (ValidationException ex)
        {
            Console.WriteLine(ex);
            return UnprocessableEntity(ex.Message);
        }
    }

    [HttpPost("{id}/subscribe")]
    public IActionResult Subscribe(string id, [FromBody] SubscribeDto dto)
    {
        try
        {
            subscribeCustomerToEventUseCase.Execute(new SubscribeCustomerToEventUseCase.Input(dto.CustomerId, id));

            return Ok();
        }
        catch (ValidationException ex)
        {
            Console.WriteLine(ex);
            return UnprocessableEntity(ex.Message);
        }
    }
}