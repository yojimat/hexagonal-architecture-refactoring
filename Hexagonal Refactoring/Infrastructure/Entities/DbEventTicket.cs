using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hexagonal_Refactoring.Application.Domain.Customer;
using Hexagonal_Refactoring.Application.Domain.Event;
using Hexagonal_Refactoring.Application.Domain.Event.Ticket;
using Microsoft.EntityFrameworkCore;
using EventId = Hexagonal_Refactoring.Application.Domain.Event.EventId;

namespace Hexagonal_Refactoring.Infrastructure.Entities;

[Table("Events")]
[PrimaryKey("EventTicketId")]
public class DbEventTicket(Guid customerId, DbEvent eEvent, int ordering)
{
    public Guid Id { get; init; }
    public Guid CustomerId { get; init; } = customerId;
    public Guid EventId { get; init; } = eEvent.Id;
    public DbEvent Event { get; init; } = eEvent;
    public int Ordering { get; init; } = ordering;

    public EventTicket ToEventTicket() => new(new TicketId(Id), new EventId(EventId), new CustomerId(CustomerId), Ordering);
}