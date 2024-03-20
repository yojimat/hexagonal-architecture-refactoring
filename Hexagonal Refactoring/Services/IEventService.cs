using Hexagonal_Refactoring.Models;

namespace Hexagonal_Refactoring.Services;

public interface IEventService
{
    public Event Save(Event ev);
    public Event? FindById(string id);
    public Ticket? FindTicketByEventIdAndCustomerId(string id, string customerId);
}