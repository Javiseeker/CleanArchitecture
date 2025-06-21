using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain.ValueObjects;

public sealed class Description : ValueObject
{
    public string Value { get; }

    private Description(string value)
    {
        Value = value;
    }

    public static Description Create(string? description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return new Description(string.Empty);

        if (description.Length > 1000)
            throw new ArgumentException("Description cannot exceed 1000 characters", nameof(description));

        return new Description(description.Trim());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    public static implicit operator string(Description description) => description.Value;
    public bool IsEmpty => string.IsNullOrEmpty(Value);
}