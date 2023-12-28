namespace Hexagonal_Refactoring.Models;

public class Ticket()
{
    private long _id;
    private Customer? _customer;
    private Event? _ev;
    private TicketStatus _status;
    private DateTime _paidAt;
    private DateTime _reservedAt;

    public Ticket(long id, Customer customer, Event ev, TicketStatus status, DateTime paidAt,
        DateTime reservedAt) : this()
    {
        _id = id;
        _customer = customer;
        _ev = ev;
        _status = status;
        _paidAt = paidAt;
        _reservedAt = reservedAt;
    }

    public long GetId()
    {
        return _id;
    }

    public void SetId(long id)
    {
        _id = id;
    }

    public Customer? GetCustomer()
    {
        return _customer;
    }

    public void SetCustomer(Customer customer)
    {
        _customer = customer;
    }

    public Event? GetEvent()
    {
        return _ev;
    }

    public void SetEvent(Event ev)
    {
        _ev = ev;
    }

    public TicketStatus GetStatus()
    {
        return _status;
    }

    public void SetStatus(TicketStatus status)
    {
        _status = status;
    }

    public DateTime GetPaidAt()
    {
        return _paidAt;
    }

    public void SetPaidAt(DateTime paidAt)
    {
        _paidAt = paidAt;
    }

    public DateTime GetReservedAt()
    {
        return _reservedAt;
    }

    public void SetReservedAt(DateTime reservedAt)
    {
        _reservedAt = reservedAt;
    }

    public override bool Equals(object? o)
    {
        if (this == o) return true;
        if (o == null || GetType() != o.GetType()) return false;
        var convertedObj = (Ticket)o;
        return Equals(_id, convertedObj._id);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_id);
    }
}
