using Hexagonal_Refactoring.Application.Domain.Event;
using Hexagonal_Refactoring.Application.Repositories;
using Hexagonal_Refactoring.Infrastructure.Contexts;
using Hexagonal_Refactoring.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using EventId = Hexagonal_Refactoring.Application.Domain.Event.EventId;

namespace Hexagonal_Refactoring.Infrastructure.Repositories;

public class EventRepository(EventContext eventContext) : IEventRepository
{
    public Event? EventOfId(EventId id)
    {
        return eventContext.Events.AsNoTracking().FirstOrDefault(e => e.Id == id.Id)?.ToEvent();
    }

    public Event Create(Event eEvent)
    {
        var newEvent = eventContext.Events.Add(DbEvent.FromEvent(eEvent));
        eventContext.SaveChanges();
        return newEvent.Entity.ToEvent();
    }

    public Event Update(Event eEvent)
    {
        throw new NotImplementedException();
    }
}