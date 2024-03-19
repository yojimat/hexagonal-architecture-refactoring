using Hexagonal_Refactoring.Application.Domain.Customer;
using Hexagonal_Refactoring.Application.Domain.Partner;

namespace Hexagonal_Refactoring.Application.Domain.Event;

public class Event
{
    private const int One = 1;

    private Event(EventId eventId, string name, string date, int totalSpots, PartnerId partnerId)
    {
        Name = name ?? throw new ArgumentException("Invalid Name for customer");
        EventId = eventId ?? throw new ArgumentException("Invalid id for event");
        Date = DateTime.Parse(date);
        TotalSpots = totalSpots;
        PartnerId = partnerId;
    }

    public EventId EventId { get; }
    public string Name { get; private set; }
    public DateTime Date { get; private set; }
    public int TotalSpots { get; }
    public PartnerId PartnerId { get; private set; }
    public HashSet<EventTicket> Tickets { get; } = [];

    public static Event NewEvent(string name, string date, int totalSpots, PartnerId partnerId) =>
        new(EventId.NewId(), name, date, totalSpots, partnerId);

    public Ticket.Ticket ReserveTicket(CustomerId customerId)
    {
        var foundTicket = Tickets.SingleOrDefault(t => t.CustomerId == customerId);
        if (foundTicket is not null) throw new InvalidOperationException("Email already registered");

        if (TotalSpots < Tickets.Count + One)
            throw new InvalidOperationException("Event sold out");

        var newTicket = Ticket.Ticket.NewTicket(customerId, EventId);

        Tickets.Add(new EventTicket(newTicket.TicketId, EventId, customerId, Tickets.Count + 1));

        return newTicket;
    }
}