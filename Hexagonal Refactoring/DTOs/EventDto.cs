using Hexagonal_Refactoring.Models;

namespace Hexagonal_Refactoring.DTOs;

public class EventDto()
{
    private string? _date;
    private long _id;
    private string? _name;
    private PartnerDto? _partner;
    private int _totalSpots;

    public EventDto(Event evnt) : this()
    {
        _id = evnt.GetId();
        _name = evnt.GetName();
        _date = evnt.GetDate().ToShortDateString();
        _totalSpots = evnt.GetTotalSpots();
        _partner = new PartnerDto(evnt.GetPartner());
    }

    public long Id
    {
        get => GetId();
        set => SetId(value);
    }

    public string? Name
    {
        get => GetName();
        set => SetName(value);
    }

    public string? Date
    {
        get => GetDate();
        set => SetDate(value);
    }

    public int TotalSpots
    {
        get => GetTotalSpots();
        set => SetTotalSpots(value);
    }

    public PartnerDto? Partner
    {
        get => GetPartner();
        set => SetPartner(value);
    }

    public long GetId()
    {
        return _id;
    }

    public void SetId(long id)
    {
        _id = id;
    }

    public string? GetName()
    {
        return _name;
    }

    public void SetName(string? name)
    {
        _name = name;
    }

    public string? GetDate()
    {
        return _date;
    }

    public void SetDate(string? date)
    {
        _date = date;
    }

    public int GetTotalSpots()
    {
        return _totalSpots;
    }

    public void SetTotalSpots(int totalSpots)
    {
        _totalSpots = totalSpots;
    }

    public PartnerDto? GetPartner()
    {
        return _partner;
    }

    public void SetPartner(PartnerDto? partner)
    {
        _partner = partner;
    }
}