namespace Hexagonal_Refactoring.Application.Domain;

// TODO: I'm thinking of implementing an abstract class for Ids.
public record TicketId(Guid Id)
{
    public static TicketId NewId() => new(Guid.NewGuid());

    public static TicketId WithId(string id)
    {
        try
        {
            return new TicketId(Guid.Parse(id));
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Invalid value for ticket id", nameof(id), ex);
        }
    }

    public override string ToString() => Id.ToString();
}