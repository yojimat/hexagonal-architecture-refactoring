using AutoMapper;
using Hexagonal_Refactoring.Models;
using Hexagonal_Refactoring.Models.DbModels;
using Hexagonal_Refactoring.Repositories.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Hexagonal_Refactoring.Repositories;

public class EventRepository(EventContext eventContext, IMapper mapper) : IEventRepository
{
    public IEnumerable<Event> GetAll()
    {
        throw new NotImplementedException();
    }

    public Event? FindById(long id)
    {
        return eventContext.Events.AsNoTracking().FirstOrDefault(e => e.Id == id);
    }

    public Event Save(Event entity)
    {
        if (entity.GetTickets() != null && entity.GetTickets()!.Any())
        {
            var ticket = entity.GetTickets()!.Single(t => t.GetId() == 0);

            var dbTicket = new DbTicket();
            mapper.Map(ticket, dbTicket);

            eventContext.Tickets.Add(dbTicket);
            eventContext.SaveChanges();
            return entity;
        }

        var dbEvent = new DbEvent();
        mapper.Map(entity, dbEvent);

        dbEvent.PartnerId = entity.GetPartner().GetId();

        var evnt = eventContext.Events.Add(dbEvent);
        eventContext.SaveChanges();
        return evnt.Entity;
    }

    public void DeleteAll()
    {
        throw new NotImplementedException();
    }
}