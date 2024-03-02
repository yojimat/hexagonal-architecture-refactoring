using Hexagonal_Refactoring.Services;

namespace Hexagonal_Refactoring.Application.UseCases;

public class GetCustomerByIdUseCase(ICustomerService customerService)
    : UseCase<GetCustomerByIdUseCase.Input, GetCustomerByIdUseCase.Output>
{
    public override Output? Execute(Input getInput)
    {
        var customer = customerService.FindById(getInput.Id);
        var output = customer == null
            ? null
            : new Output(customer.GetId(), customer.GetCpf(), customer.GetEmail(), customer.GetName());
        return output;
    }

    public record Input(long Id);

    public record Output(long Id, string Cpf, string Email, string Name);
}