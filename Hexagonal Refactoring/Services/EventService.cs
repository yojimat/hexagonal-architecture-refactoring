using Hexagonal_Refactoring.Models;
using Hexagonal_Refactoring.Repositories;

namespace Hexagonal_Refactoring.Services;

public class EventService(IEventRepository eventRepository, ITicketRepository ticketRepository)
    : IEventService
{
    public Event Save(Event ev)
    {
        return eventRepository.Save(ev);
    }

    public Event? FindById(long id)
    {
        return eventRepository.FindById(id);
    }

    public Ticket? FindTicketByEventIdAndCustomerId(long id, long customerId)
    {
        return ticketRepository.FindByEventIdAndCustomerId(id, customerId);
    }
}