namespace Hexagonal_Refactoring.Application.UseCases;

public abstract class NullaryUseCase<TOutput>
{
    public abstract TOutput Execute();
}