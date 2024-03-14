namespace Hexagonal_Refactoring.Application.Domain;

// TODO: I'm thinking of implementing an abstract class for Ids.
public record CustomerId(Guid Id)
{
    public static CustomerId NewId() => new(Guid.NewGuid());

    public static CustomerId WithId(string id)
    {
        try
        {
            return new CustomerId(Guid.Parse(id));
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Invalid value for customer id", nameof(id), ex);
        }
    }

    public override string ToString() => Id.ToString();
}