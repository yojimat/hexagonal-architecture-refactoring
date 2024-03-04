namespace Hexagonal_Refactoring.Application.Entities;

// TODO: I'm thinking of implementing an abstract class for Ids.
public record PartnerId(Guid Id)
{
    public static PartnerId NewId() => new(Guid.NewGuid());

    public static PartnerId WithId(string id)
    {
        try
        {
            return new PartnerId(Guid.Parse(id));
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Invalid value for partner id", nameof(id), ex);
        }
    }

    public override string ToString() => Id.ToString();
}