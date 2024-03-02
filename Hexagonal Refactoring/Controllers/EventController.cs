using System.ComponentModel.DataAnnotations;
using Hexagonal_Refactoring.Application.UseCases;
using Hexagonal_Refactoring.DTOs;
using Hexagonal_Refactoring.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hexagonal_Refactoring.Controllers;

[ApiController]
[Route("api/events")]
public class EventController(
    CreateEventUseCase createEventUseCase,
    SubscribeCustomerToEventUseCase subscribeCustomerToEventUseCase) : ControllerBase
{
    [HttpPost]
    public IActionResult Create([FromBody] EventDto dto)
    {
        if (dto.Partner is null) return BadRequest("Partner is required");

        try
        {
            var output = createEventUseCase.Execute(new CreateEventUseCase.Input(dto.GetName() ?? string.Empty,
                dto.GetDate() ?? string.Empty, dto.GetTotalSpots(),
                dto.Partner.GetId()));

            return Created($"/events/{output.Id}",
                new EventDto(new Event(output.Id, dto.GetName(), DateTime.Parse(dto.GetDate() ?? string.Empty),
                    dto.GetTotalSpots(), new HashSet<Ticket>())));
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
            subscribeCustomerToEventUseCase.Execute(new SubscribeCustomerToEventUseCase.Input(id, dto.Id));

            return Ok();
        }
        catch (ValidationException ex)
        {
            Console.WriteLine(ex);
            return UnprocessableEntity(ex.Message);
        }
    }
}