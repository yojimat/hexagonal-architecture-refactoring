using DomainEvent = Hexagonal_Refactoring.Application.Domain.Event;
using Hexagonal_Refactoring.Application.Domain.Partner;
using Hexagonal_Refactoring.Application.Repositories;

namespace Hexagonal_Refactoring.Application.UseCases.Event;

public class CreateEventUseCase(IPartnerRepository partnerRepository, IEventRepository eventRepository)
    : UseCase<CreateEventUseCase.Input, CreateEventUseCase.Output>
{
    public override Output Execute(Input input)
    {
        var partner = partnerRepository.PartnerOfId(PartnerId.WithId(input.PartnerId)) ??
                      throw new Exception("Partner not found");

        var newEvent =
            eventRepository.Create(DomainEvent.Event.NewEvent(input.Name, input.Date, input.TotalSpots, partner.PartnerId));

        return new Output(newEvent.EventId.ToString(), newEvent.PartnerId.ToString());
    }

    public record Input(string Name, string Date, int TotalSpots, string PartnerId);

    public record Output(string EventId, string PartnerId);
}