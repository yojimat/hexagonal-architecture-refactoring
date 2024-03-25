using System.ComponentModel.DataAnnotations.Schema;
using Hexagonal_Refactoring.Application.Domain.Event;
using Hexagonal_Refactoring.Application.Domain.Event.Ticket;
using Microsoft.EntityFrameworkCore;

namespace Hexagonal_Refactoring.Infrastructure.Entities;

[Table("Tickets")]
[PrimaryKey("Id")]
public class DbTicket(Guid customerId, Guid eventId, TicketStatus status, DateTime? paidAt, DateTime reservedAt)
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; init; }

    public Guid CustomerId { get; init; } = customerId;
    public Guid EventId { get; init; } = eventId;
    public TicketStatus Status { get; init; } = status;
    public DateTime? PaidAt { get; init; } = paidAt;
    public DateTime ReservedAt { get; init; } = reservedAt;

    public static DbTicket FromTicket(Ticket ticket) =>
       new(ticket.CustomerId.Id, ticket.EventId.Id, ticket.Status, ticket.PaidAt, ticket.ReservedAt);

    public Ticket ToTicket() =>
        Ticket.Restore(Id, CustomerId, EventId, Status, PaidAt, ReservedAt);
}