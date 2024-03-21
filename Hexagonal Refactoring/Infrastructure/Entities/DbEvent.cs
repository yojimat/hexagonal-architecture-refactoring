using System.ComponentModel.DataAnnotations.Schema;
using Hexagonal_Refactoring.Application.Domain.Event;
using Hexagonal_Refactoring.Application.Domain.Partner;
using Microsoft.EntityFrameworkCore;
using EventId = Hexagonal_Refactoring.Application.Domain.Event.EventId;

namespace Hexagonal_Refactoring.Infrastructure.Entities;

[Table("Events")]
[PrimaryKey("EventId")]
public class DbEvent(Guid partnerId, string name, DateTime date, int totalSpots)
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; init; }

    public Guid PartnerId { get; init; } = partnerId;

    [Column(TypeName = "varchar(100)")]
    public string Name { get; init; } = name;
    public DateTime Date { get; init; } = date;
    public int TotalSpots { get; init; } = totalSpots;
    public List<DbEventTicket> Tickets { get; init; } = [];

    public static DbEvent FromEvent(Event eEvent)
    {
        var entity = new DbEvent(eEvent.PartnerId.Id, eEvent.Name, eEvent.Date, eEvent.TotalSpots);
        var ticketsList = eEvent.Tickets.ToList();
        ticketsList.ForEach(t => entity.Tickets.Add(new DbEventTicket(t.CustomerId.Id, entity, t.Ordering)));
        return entity;
    }

    public Event ToEvent() =>
        Event.Restore(new EventId(Id), Name, Date, TotalSpots, new PartnerId(PartnerId),
            Tickets.Select(t => t.ToEventTicket()));
}
