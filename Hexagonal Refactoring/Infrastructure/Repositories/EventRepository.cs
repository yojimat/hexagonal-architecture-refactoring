using Hexagonal_Refactoring.Application.Domain.Event;
using Hexagonal_Refactoring.Application.Repositories;
using Hexagonal_Refactoring.Infrastructure.Contexts;
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
        var dbEvent = new DbEvent();
        mapper.Map(entity, dbEvent);

        dbEvent.PartnerId = entity.GetPartner().GetId();

        var evnt = eventContext.Events.Add(dbEvent);
        eventContext.SaveChanges();
        return evnt.Entity;
    }

    public Event Update(Event eEvent)
    {
        throw new NotImplementedException();
    }
}