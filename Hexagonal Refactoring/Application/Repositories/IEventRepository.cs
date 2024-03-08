using Hexagonal_Refactoring.Application.Entities;
using EventId = Hexagonal_Refactoring.Application.Entities.EventId;

namespace Hexagonal_Refactoring.Application.Repositories;

public interface IEventRepository
{
    Event? EventOfId(EventId id);
    Event Create(Event eEvent);
    Event Update(Event eEvent);
}