using Hexagonal_Refactoring.Application.Domain.Event.Ticket;

namespace Tests_Hexagonal_Refactoring.InMemoryRepositories;

public class InMemoryTicketRepository : ITicketRepository
{
    private readonly Dictionary<string, Ticket> _tickets = [];

    public Ticket? TicketOfId(TicketId id) => _tickets.GetValueOrDefault(id.Id.ToString());

    public Ticket Create(Ticket ticket)
    {
        _tickets.Add(ticket.TicketId.ToString(), ticket);
        return ticket;
    }

    public Ticket Update(Ticket ticket)
    {
        _tickets.Add(ticket.TicketId.ToString(), ticket);
        return ticket;
    }
}