namespace SixShaded.FourZeroOne.Rule;

using System.Diagnostics.CodeAnalysis;

public readonly struct RuleID(int id)
{
    public readonly int ID = id;
    public override string ToString() => $"RuleID({ID})";
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is RuleID other && other.ID == ID;
    public override int GetHashCode() => ID.GetHashCode();
}