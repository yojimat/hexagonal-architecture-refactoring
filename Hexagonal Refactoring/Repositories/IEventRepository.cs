using Hexagonal_Refactoring.Models;

namespace Hexagonal_Refactoring.Repositories;

public interface IEventRepository : ICrudRepository<Event, string>;