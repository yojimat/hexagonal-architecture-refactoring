using Hexagonal_Refactoring.Application.Domain.Event.Ticket;
using Hexagonal_Refactoring.Application.Repositories;
using Hexagonal_Refactoring.Infrastructure.Contexts;
using Hexagonal_Refactoring.Infrastructure.Entities;

namespace Hexagonal_Refactoring.Infrastructure.Repositories;

public class TicketRepository(EventContext eventContext) : ITicketRepository
{
    public Ticket? TicketOfId(TicketId id) => eventContext.Tickets.Find(id)?.ToTicket();

    public Ticket Create(Ticket ticket)
    {
        var newTicket= eventContext.Tickets.Add(DbTicket.FromTicket(ticket));
        eventContext.SaveChanges();
        return newTicket.Entity.ToTicket();
    }

    public Ticket Update(Ticket ticket)
    {
        throw new NotImplementedException();
    }
}