using System.ComponentModel.DataAnnotations;
using Hexagonal_Refactoring.Models;
using Hexagonal_Refactoring.Services;

namespace Hexagonal_Refactoring.Application.UseCases;

public class SubscribeCustomerToEventUseCase(ICustomerService customerService, IEventService eventService) :
    UnitUseCase<SubscribeCustomerToEventUseCase.Input>
{
    public record Input(long CustomerId, long EventId);

    public override void Execute(Input input)
    {
        var maybeEvent = eventService.FindById(input.EventId) ??
                            throw new ValidationException("Event not found");

        var maybeCustomer = customerService.FindById(input.CustomerId) ??
                            throw new ValidationException("Customer not found");

        _ = eventService.FindTicketByEventIdAndCustomerId(input.EventId, input.CustomerId) ??
            throw new ValidationException("Email already registered");

        if (maybeEvent.GetTotalSpots() < maybeEvent.GetTickets()!.Count + 1)
            throw new ValidationException("Event sold out");

        var ticket = new Ticket();
        ticket.SetEvent(maybeEvent);
        ticket.SetCustomer(maybeCustomer);
        ticket.SetReservedAt(DateTime.Now);
        ticket.SetStatus(TicketStatus.Pending);

        maybeEvent.GetTickets()!.Add(ticket);

        eventService.Save(maybeEvent);
    }
}