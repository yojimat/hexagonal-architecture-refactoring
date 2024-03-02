using Hexagonal_Refactoring.Models;
using Hexagonal_Refactoring.Models.DbModels;
using Hexagonal_Refactoring.Repositories.Contexts;

namespace Hexagonal_Refactoring.Repositories;

public class TicketRepository(EventContext eventContext) : ITicketRepository
{
    public IEnumerable<Ticket> GetAll()
    {
        throw new NotImplementedException();
    }

    public Ticket? FindById(long id)
    {
        return eventContext.Tickets.Find(id);
    }

    public Ticket Save(Ticket entity)
    {
        var ticketEntityEntry =
            eventContext.Tickets.Add(entity as DbTicket ?? throw new ArgumentNullException(nameof(entity)));
        eventContext.SaveChanges();
        return ticketEntityEntry.Entity;
    }

    public void DeleteAll()
    {
        throw new NotImplementedException();
    }

    public Ticket? FindByEventIdAndCustomerId(long eventId, long customerId)
    {
        return eventContext.Tickets.FirstOrDefault(p =>
            p.EventId == eventId && p.CustomerId == customerId);
    }
}