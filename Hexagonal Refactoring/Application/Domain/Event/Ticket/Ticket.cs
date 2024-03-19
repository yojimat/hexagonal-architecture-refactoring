using Hexagonal_Refactoring.Application.Domain.Customer;
using Hexagonal_Refactoring.Models;

namespace Hexagonal_Refactoring.Application.Domain.Event.Ticket;

public class Ticket
{
    private Ticket(TicketId ticketId, CustomerId customerId, EventId eventId)
    {
        TicketId = ticketId;
        CustomerId = customerId;
        EventId = eventId;
    }

    public TicketId TicketId { get; private set; }
    public CustomerId CustomerId { get; private set; }
    public EventId EventId { get; private set; }
    public TicketStatus Status { get; private set; } = TicketStatus.Pending;
    public DateTime? PaidAt { get; }
    public DateTime ReservedAt { get; private set; } = DateTime.Now;

    public static Ticket NewTicket(CustomerId customerId, EventId eventId) =>
        new(TicketId.NewId(), customerId, eventId);
}