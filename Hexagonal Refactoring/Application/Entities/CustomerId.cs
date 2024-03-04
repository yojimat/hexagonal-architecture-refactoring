namespace Hexagonal_Refactoring.Application.Entities;

public record CustomerId(Guid Id)
{
    public static CustomerId NewId()
    {
        return new CustomerId(Guid.NewGuid());
    }

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

    public override string ToString()
    {
        return Id.ToString();
    }
}