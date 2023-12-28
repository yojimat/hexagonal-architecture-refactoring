namespace Hexagonal_Refactoring.DTOs;

public class SubscribeDto
{
    private long _customerId;
    public long Id { get => GetCustomerId(); set => SetCustomerId(value); }

    public long GetCustomerId()
    {
        return _customerId;
    }

    public void SetCustomerId(long customerId)
    {
        _customerId = customerId;
    }
}
