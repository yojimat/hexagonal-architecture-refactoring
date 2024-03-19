namespace Hexagonal_Refactoring.Application.UseCases;

public abstract class UseCase<TInput, TOutput>
{
    public abstract TOutput? Execute(TInput input);
}