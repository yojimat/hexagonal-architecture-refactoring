namespace Hexagonal_Refactoring.Application;

public abstract class UnitUseCase<TInput>
{
    public abstract void Execute(TInput input);
}