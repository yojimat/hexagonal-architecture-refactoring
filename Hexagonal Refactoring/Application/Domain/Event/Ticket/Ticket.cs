using Hexagonal_Refactoring.Application.Domain.Customer;

namespace Hexagonal_Refactoring.Application.Domain.Event.Ticket;

public class Ticket
{
    private Ticket(TicketId ticketId, CustomerId customerId, EventId eventId)
    {
        TicketId = ticketId;
        CustomerId = customerId;
        EventId = eventId;
    }

    private Ticket(TicketId ticketId, CustomerId customerId, EventId eventId, TicketStatus status, DateTime? paidAt, DateTime reservedAt)
    {
        TicketId = ticketId;
        CustomerId = customerId;
        EventId = eventId;
        Status = status;
        PaidAt = paidAt;
        ReservedAt = reservedAt;
    }

    public TicketId TicketId { get; private set; }
    public CustomerId CustomerId { get; private set; }
    public EventId EventId { get; private set; }
    public TicketStatus Status { get; private set; } = TicketStatus.Pending;
    public DateTime? PaidAt { get; }
    public DateTime ReservedAt { get; private set; } = DateTime.Now;

    public static Ticket NewTicket(CustomerId customerId, EventId eventId) =>
        new(TicketId.NewId(), customerId, eventId);

    public static Ticket Restore(Guid id, Guid customerId, Guid eventId, TicketStatus status, DateTime? paidAt, DateTime reservedAt) => 
        new(new TicketId(id), new CustomerId(customerId), new EventId(eventId), status, paidAt, reservedAt);
}