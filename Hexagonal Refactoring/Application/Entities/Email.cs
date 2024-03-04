using System.Text.RegularExpressions;

namespace Hexagonal_Refactoring.Application.Entities;

public partial record Email
{
    public string Value { get; }

    public Email(string value)
    {
        var emailIsValid = EmailValidation().IsMatch(value);
        if (!emailIsValid)
            throw new ArgumentException("Invalid Email", value);

        Value = value;
    }

    public override string ToString() => Value;

    [GeneratedRegex(@"^\S+@\S+\.\S+$")]
    private static partial Regex EmailValidation();
}