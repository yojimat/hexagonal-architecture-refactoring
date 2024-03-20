using Hexagonal_Refactoring.Models;

namespace Hexagonal_Refactoring.Repositories;

public interface ITicketRepository : ICrudRepository<Ticket, string>
{
    Ticket? FindByEventIdAndCustomerId(string eventId, string customerId);
}