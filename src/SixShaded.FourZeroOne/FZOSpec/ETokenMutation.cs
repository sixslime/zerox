
namespace SixShaded.FourZeroOne.FZOSpec;

public abstract record ETokenMutation
{
    public required Tok Result { get; init; }

    public sealed record Identity : ETokenMutation { }

    public sealed record RuleApply : ETokenMutation
    {
        public required Rul Rule { get; init; }
    }
}