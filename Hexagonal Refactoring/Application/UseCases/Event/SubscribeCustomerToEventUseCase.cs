using System.ComponentModel.DataAnnotations;
using Hexagonal_Refactoring.Application.Domain.Customer;
using Hexagonal_Refactoring.Application.Repositories;
using EventId = Hexagonal_Refactoring.Application.Domain.Event.EventId;

namespace Hexagonal_Refactoring.Application.UseCases.Event;

public class SubscribeCustomerToEventUseCase(
    ICustomerRepository customerRepository,
    IEventRepository eventRepository,
    ITicketRepository ticketRepository) :
    UseCase<SubscribeCustomerToEventUseCase.Input, SubscribeCustomerToEventUseCase.Output>
{
    public override Output Execute(Input input)
    {
        var maybeEvent = eventRepository.EventOfId(EventId.WithId(input.EventId)) ??
                         throw new ValidationException("Event not found");

        var maybeCustomer = customerRepository.CustomerOfId(CustomerId.WithId(input.CustomerId)) ??
                            throw new ValidationException("Customer not found");


        var ticket = maybeEvent.ReserveTicket(maybeCustomer.CustomerId);
        ticketRepository.Create(ticket);
        eventRepository.Update(maybeEvent);
        return new Output(ticket.TicketId.ToString());
    }

    public record Input(string CustomerId, string EventId);

    public record Output(string TicketId);
}