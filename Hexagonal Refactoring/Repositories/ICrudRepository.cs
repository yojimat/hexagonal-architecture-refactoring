namespace Hexagonal_Refactoring.Repositories;

public interface ICrudRepository<T, TK>
{
    IEnumerable<T> GetAll();
    T? FindById(TK id);
    T Save(T entity);
    void DeleteAll();
}