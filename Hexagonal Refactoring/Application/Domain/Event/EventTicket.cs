using Hexagonal_Refactoring.Application.Domain.Customer;
using Hexagonal_Refactoring.Application.Domain.Event.Ticket;

namespace Hexagonal_Refactoring.Application.Domain.Event;

public class EventTicket(TicketId ticketId, EventId eventId, CustomerId customerId, int ordering)
{
    public TicketId TicketId { get; private set; } = ticketId;
    public EventId EventId { get; private set; } = eventId;
    public CustomerId CustomerId { get; private set; } = customerId;
    public int Ordering { get; private set; } = ordering;
}