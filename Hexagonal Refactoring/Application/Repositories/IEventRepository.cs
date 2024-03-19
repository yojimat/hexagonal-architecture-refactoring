using Hexagonal_Refactoring.Application.Domain.Event;
using EventId = Hexagonal_Refactoring.Application.Domain.Event.EventId;

namespace Hexagonal_Refactoring.Application.Repositories;

public interface IEventRepository
{
    Event? EventOfId(EventId id);
    Event Create(Event eEvent);
    Event Update(Event eEvent);
}