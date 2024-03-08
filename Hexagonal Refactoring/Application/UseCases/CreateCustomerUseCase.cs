using System.ComponentModel.DataAnnotations;
using Hexagonal_Refactoring.Application.Entities;
using Hexagonal_Refactoring.Application.Repositories;

namespace Hexagonal_Refactoring.Application.UseCases;

public class CreateCustomerUseCase(ICustomerRepository customerRepository)
    : UseCase<CreateCustomerUseCase.Input, CreateCustomerUseCase.Output>
{
    public override Output Execute(Input input)
    {
        if (customerRepository.CustomerOfCpf(input.Cpf) != null)
            throw new ValidationException("Customer CPF already in use");

        if (customerRepository.CustomerOfEmail(input.Email) != null)
            throw new ValidationException("Customer Email already in use");

        var customer = customerRepository.Create(Customer.NewCustomer(input.Name, input.Cpf, input.Email));

        return new Output(customer.CustomerId.ToString(), customer.Cpf, customer.Email.Value,
            customer.Name);
    }

    public record Input(string Cpf, string Email, string Name);
    public record Output(string Id, string Cpf, string Email, string Name);
}