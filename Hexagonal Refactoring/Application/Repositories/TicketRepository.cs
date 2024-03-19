using Hexagonal_Refactoring.Application.Domain.Event.Ticket;

namespace Hexagonal_Refactoring.Application.Repositories;

public interface ITicketRepository
{
    Ticket? TicketOfId(TicketId id);
    Ticket Create(Ticket ticket);
    Ticket Update(Ticket ticket);
}