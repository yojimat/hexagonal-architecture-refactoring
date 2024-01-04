namespace Hexagonal_Refactoring.Application;

public abstract class NullaryUseCase<TOutput>
{
    public abstract TOutput Execute();
}
