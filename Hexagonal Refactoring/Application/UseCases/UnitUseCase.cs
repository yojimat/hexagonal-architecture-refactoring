namespace Hexagonal_Refactoring.Application.UseCases;

public abstract class UnitUseCase<TInput>
{
    public abstract void Execute(TInput input);
}