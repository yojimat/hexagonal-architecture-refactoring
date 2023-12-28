using Hexagonal_Refactoring.Models;

namespace Hexagonal_Refactoring.Repositories;

public interface ITicketRepository : ICrudRepository<Ticket, long>
{
    Ticket? FindByEventIdAndCustomerId(long eventId, long customerId);
}
