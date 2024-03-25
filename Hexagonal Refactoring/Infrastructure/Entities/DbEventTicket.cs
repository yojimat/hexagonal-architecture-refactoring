using System.ComponentModel.DataAnnotations.Schema;
using Hexagonal_Refactoring.Application.Domain.Customer;
using Hexagonal_Refactoring.Application.Domain.Event;
using Hexagonal_Refactoring.Application.Domain.Event.Ticket;
using Microsoft.EntityFrameworkCore;
using EventId = Hexagonal_Refactoring.Application.Domain.Event.EventId;

namespace Hexagonal_Refactoring.Infrastructure.Entities;

[Table("EventTickets")]
[PrimaryKey("Id")]
public class DbEventTicket(Guid ticketId, Guid customerId, Guid eventId, int ordering)
{
    public Guid Id { get; init; }

    public Guid TicketId { get; init; } = ticketId;
    public Guid CustomerId { get; init; } = customerId;
    public Guid EventId { get; init; } = eventId;

    public int Ordering { get; init; } = ordering;

    public EventTicket ToEventTicket() => new(new TicketId(TicketId), new EventId(EventId), new CustomerId(CustomerId), Ordering);
}