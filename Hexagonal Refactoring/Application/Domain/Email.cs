using System.Text.RegularExpressions;

namespace Hexagonal_Refactoring.Application.Domain;

public partial record Email
{
    public Email(string value)
    {
        var emailIsValid = EmailValidation().IsMatch(value);
        if (!emailIsValid)
            throw new ArgumentException("Invalid Email", value);

        Value = value;
    }

    public string Value { get; }

    public override string ToString() => Value;

    [GeneratedRegex(@"^\S+@\S+\.\S+$")]
    private static partial Regex EmailValidation();
}