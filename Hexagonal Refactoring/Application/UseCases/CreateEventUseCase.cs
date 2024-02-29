using System.ComponentModel.DataAnnotations;
using Hexagonal_Refactoring.Models;
using Hexagonal_Refactoring.Services;

namespace Hexagonal_Refactoring.Application.UseCases;

public class CreateEventUseCase(IPartnerService partnerService, IEventService eventService) : UseCase<CreateEventUseCase.Input, CreateEventUseCase.Output>
{
    public record Input(string Name, string Date, long TotalSpots, long PartnerId);
    public record Output(long Id, long PartnerId);

    public override Output Execute(Input input)
    {
        var partner = partnerService.FindById(input.PartnerId);

        if (partner == null) throw new ValidationException("Partner not found");

        var evnt = new Event();
        evnt.SetDate(DateTime.Parse(input.Date));
        evnt.SetName(input.Name);
        evnt.SetTotalSpots((int)input.TotalSpots);
        evnt.SetPartner(partner);

        var newEvent = eventService.Save(evnt);

        return new Output(newEvent.GetId(), newEvent.GetPartner().GetId());
    }
}