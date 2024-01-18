namespace Hexagonal_Refactoring.Application;

public abstract class UseCase<TInput, TOutput>
{
    public abstract TOutput? Execute(TInput input);
}

