using Hexagonal_Refactoring.Application.Domain;
using Hexagonal_Refactoring.Application.Repositories;

namespace Hexagonal_Refactoring.Application.UseCases;

public class CreateEventUseCase(IPartnerRepository partnerRepository, IEventRepository eventRepository)
    : UseCase<CreateEventUseCase.Input, CreateEventUseCase.Output>
{
    public override Output Execute(Input input)
    {
        var partner = partnerRepository.PartnerOfId(PartnerId.WithId(input.PartnerId)) ??
                      throw new Exception("Partner not found");

        var newEvent =
            eventRepository.Create(Event.NewEvent(input.Name, input.Date, input.TotalSpots, partner.PartnerId));

        return new Output(newEvent.EventId.ToString(), newEvent.PartnerId.ToString());
    }

    public record Input(string Name, string Date, int TotalSpots, string PartnerId);

    public record Output(string EventId, string PartnerId);
}