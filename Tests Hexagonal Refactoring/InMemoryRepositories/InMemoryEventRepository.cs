using Hexagonal_Refactoring.Application.Domain.Event;

namespace Tests_Hexagonal_Refactoring.InMemoryRepositories;

public class InMemoryEventRepository : IEventRepository
{
    private readonly Dictionary<string, Event> _events = [];

    public Event? EventOfId(EventId id) => _events.GetValueOrDefault(id.Id.ToString());

    public Event Create(Event eEvent)
    {
        _events.Add(eEvent.EventId.ToString(), eEvent);
        return eEvent;
    }

    public Event Update(Event eEvent)
    {
        _events[eEvent.EventId.ToString()] = eEvent;
        return eEvent;
    }
}