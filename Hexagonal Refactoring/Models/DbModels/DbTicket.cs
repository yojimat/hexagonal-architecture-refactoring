using System.ComponentModel.DataAnnotations.Schema;
using Hexagonal_Refactoring.Application.Domain.Event;
using Microsoft.EntityFrameworkCore;

namespace Hexagonal_Refactoring.Models.DbModels;

[Table("Tickets")]
[PrimaryKey("EventId")]
public class DbTicket : Ticket
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id
    {
        get => GetId();
        init => SetId(value);
    }

    public string CustomerId { get; init; }
    public string EventId { get; init; }

    public TicketStatus Status
    {
        get => GetStatus();
        init => SetStatus(value);
    }

    public DateTime PaidAt
    {
        get => GetPaidAt();
        init => SetPaidAt(value);
    }

    public DateTime ReservedAt
    {
        get => GetReservedAt();
        init => SetReservedAt(value);
    }
}