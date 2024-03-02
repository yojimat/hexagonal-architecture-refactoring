using Hexagonal_Refactoring.Models;

namespace Hexagonal_Refactoring.Services;

public interface IEventService
{
    public Event Save(Event ev);
    public Event? FindById(long id);
    public Ticket? FindTicketByEventIdAndCustomerId(long id, long customerId);
}