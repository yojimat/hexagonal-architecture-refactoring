using System.ComponentModel.DataAnnotations;
using Hexagonal_Refactoring.Models;
using Hexagonal_Refactoring.Services;

namespace Hexagonal_Refactoring.Application.UseCases;

public class CreateCustomerUseCase(ICustomerService customerService) : UseCase<CreateCustomerUseCase.Input, CreateCustomerUseCase.Output>
{
    public record Input(string Cpf, string Email, string Name);
    public record Output(long Id, string Cpf, string Email, string Name);

    public override Output Execute(Input input)
    {
        if (customerService.FindByCpf(input.Cpf) != null)
            throw new ValidationException("Customer already exists.");

        if (customerService.FindByEmail(input.Email) != null)
            throw new ValidationException("Customer already exists.");

        var customer = new Customer();
        customer.SetName(input.Name);
        customer.SetCpf(input.Cpf);
        customer.SetEmail(input.Email);

        customer = customerService.Save(customer);

        return new Output(customer.GetId(), customer.GetCpf(), customer.GetEmail(), customer.GetName());
    }
}
