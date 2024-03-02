namespace Hexagonal_Refactoring.Models;

public class Event()
{
    private DateTime _date;
    private long _id;
    private string? _name;
    private Partner _partner = new();
    private ISet<Ticket>? _tickets = new HashSet<Ticket>();
    private int _totalSpots;

    public Event(long id, string? name, DateTime date, int totalSpots, ISet<Ticket>? tickets) : this()
    {
        _id = id;
        _name = name;
        _date = date;
        _totalSpots = totalSpots;
        _tickets = tickets;
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

    public DateTime GetDate()
    {
        return _date;
    }

    public void SetDate(DateTime date)
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

    public Partner GetPartner()
    {
        return _partner;
    }

    public void SetPartner(Partner partner)
    {
        _partner = partner;
    }

    public ISet<Ticket>? GetTickets()
    {
        return _tickets;
    }

    public void SetTickets(ISet<Ticket> tickets)
    {
        _tickets = tickets;
    }

    public override bool Equals(object? o)
    {
        if (this == o) return true;
        if (o == null || GetType() != o.GetType()) return false;
        var convertedObj = (Event)o;
        return Equals(_id, convertedObj._id);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_id);
    }
}