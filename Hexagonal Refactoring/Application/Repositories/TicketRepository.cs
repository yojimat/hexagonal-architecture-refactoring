using Hexagonal_Refactoring.Application.Domain;

namespace Hexagonal_Refactoring.Application.Repositories;

public interface ITicketRepository
{
    Ticket? TicketOfId(TicketId id);
    Ticket Create(Ticket ticket);
    Ticket Update(Ticket ticket);
}