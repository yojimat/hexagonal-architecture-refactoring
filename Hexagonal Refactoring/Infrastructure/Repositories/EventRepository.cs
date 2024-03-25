using Hexagonal_Refactoring.Application.Domain.Event;
using Hexagonal_Refactoring.Application.Domain.Event.Ticket;
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
        var foundEvent = eventContext.Events.Single(e => e.Id == eEvent.EventId.Id);
        foundEvent.Tickets.AddRange(eEvent.Tickets.Select(t => 
            new DbEventTicket(t.TicketId.Id, t.CustomerId.Id, t.EventId.Id, t.Ordering)));
        var updatedEvent = eventContext.Events.Update(foundEvent);
        eventContext.SaveChanges();
        return updatedEvent.Entity.ToEvent();
    }
}