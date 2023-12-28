using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Hexagonal_Refactoring.Models.DbModels;

[Table("Tickets"), PrimaryKey("Id")]
public class DbTicket : Ticket
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id
    {
        get => GetId();
        init => SetId(value);
    }

    public long CustomerId { get; init; }
    public long EventId { get; init; }

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
