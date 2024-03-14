namespace Hexagonal_Refactoring.Application.Domain;

// TODO: I'm thinking of implementing an abstract class for Ids.
public record EventId(Guid Id)
{
    public static EventId NewId() => new(Guid.NewGuid());

    public static EventId WithId(string id)
    {
        try
        {
            return new EventId(Guid.Parse(id));
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Invalid value for event id", nameof(id), ex);
        }
    }

    public override string ToString() => Id.ToString();
}