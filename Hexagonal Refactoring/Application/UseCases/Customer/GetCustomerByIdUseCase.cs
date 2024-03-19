using Hexagonal_Refactoring.Application.Domain.Customer;
using Hexagonal_Refactoring.Application.Repositories;

namespace Hexagonal_Refactoring.Application.UseCases.Customer;

public class GetCustomerByIdUseCase(ICustomerRepository customerRepository)
    : UseCase<GetCustomerByIdUseCase.Input, GetCustomerByIdUseCase.Output>
{
    public override Output? Execute(Input input)
    {
        var customer = customerRepository.CustomerOfId(CustomerId.WithId(input.Id));
        var output = customer == null
            ? null
            : new Output(customer.CustomerId.ToString(), customer.Cpf, customer.Email.Value, customer.Name);
        return output;
    }

    public record Input(string Id);

    public record Output(string Id, string Cpf, string Email, string Name);
}