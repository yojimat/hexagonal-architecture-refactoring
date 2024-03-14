using Hexagonal_Refactoring.Application.Domain;
using EventId = Hexagonal_Refactoring.Application.Domain.EventId;

namespace Hexagonal_Refactoring.Application.Repositories;

public interface IEventRepository
{
    Event? EventOfId(EventId id);
    Event Create(Event eEvent);
    Event Update(Event eEvent);
}