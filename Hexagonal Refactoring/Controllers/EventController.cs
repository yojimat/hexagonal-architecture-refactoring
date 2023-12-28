using Hexagonal_Refactoring.DTOs;
using Hexagonal_Refactoring.Models;
using Hexagonal_Refactoring.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hexagonal_Refactoring.Controllers;

[ApiController]
[Route("api/events")]
public class EventController(IEventService eventService, IPartnerService partnerService, ICustomerService customerService) : ControllerBase
{
    [HttpPost]
    public IActionResult Create([FromBody] EventDto dto)
    {
        var evnt = new Event();
        evnt.SetDate(DateTime.Parse(dto.GetDate() ?? DateTime.Now.ToShortDateString()));
        evnt.SetName(dto.GetName());
        evnt.SetTotalSpots(dto.GetTotalSpots());

        var partner = partnerService.FindById(dto.GetPartner()!.GetId());

        if (partner == null) { throw new Exception("Partner not found"); }

        evnt.SetPartner(partner);
        return Created("", new EventDto(eventService.Save(evnt)));
    }

    [HttpPost("{id:long}/subscribe")]
    public IActionResult Subscribe(long id, [FromBody] SubscribeDto dto)
    {
        var maybeCustomer = customerService.FindById(dto.GetCustomerId());
        if (maybeCustomer is null)
        {
            return UnprocessableEntity("Customer not found");
        }

        var maybeEvent = eventService.FindById(id);
        if (maybeEvent is null)
        {
            return NotFound();
        }

        var maybeTicket = eventService.FindTicketByEventIdAndCustomerId(id, dto.GetCustomerId());
        if (maybeTicket is not null)
        {
            return UnprocessableEntity("Email already registered");
        }

        if (maybeEvent.GetTotalSpots() < maybeEvent.GetTickets()!.Count + 1)
        {
            throw new Exception("Event sold out");
        }

        var ticket = new Ticket();
        ticket.SetEvent(maybeEvent);
        ticket.SetCustomer(maybeCustomer);
        ticket.SetReservedAt(DateTime.Now);
        ticket.SetStatus(TicketStatus.Pending);

        maybeEvent.GetTickets()!.Add(ticket);

        eventService.Save(maybeEvent);

        return Ok(new EventDto(maybeEvent));
    }
}
