namespace Hexagonal_Refactoring.Application.Entities;

public class Event
{
    private Event(EventId eventId, string name, string date, int totalSpots, PartnerId partnerId)
    {
        Name = name ?? throw new ArgumentException("Invalid Name for customer");
        EventId = eventId ?? throw new ArgumentException("Invalid id for event");
        Date = DateTime.Parse(date );
        TotalSpots = totalSpots;
        PartnerId = partnerId;
    }

    public EventId EventId { get; private set; }
    public string Name { get; private set; }
    public DateTime Date { get; private set; }
    public int TotalSpots { get; private set; }
    public PartnerId PartnerId { get; private set; }

    public static Event NewEvent(string name, string date, int totalSpots, PartnerId partnerId) =>
        new(EventId.NewId(), name, date, totalSpots, partnerId);
}