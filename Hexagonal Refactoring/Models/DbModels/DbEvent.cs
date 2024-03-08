using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Hexagonal_Refactoring.Models.DbModels;

[Table("Events")]
[PrimaryKey("EventId")]
public class DbEvent : Event
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id
    {
        get => GetId();
        init => SetId(value);
    }

    public string PartnerId
    {
        get => GetPartnerId();
        set => SetPartnerId(value);
    }

    public string? Name
    {
        get => GetName();
        init => SetName(value ?? "");
    }

    public DateTime Date
    {
        get => GetDate();
        init => SetDate(value);
    }

    public int TotalSpots
    {
        get => GetTotalSpots();
        init => SetTotalSpots(value);
    }

    private string GetPartnerId()
    {
        return GetPartner().GetId();
    }

    private void SetPartnerId(string value)
    {
        GetPartner().SetId(value);
    }
}