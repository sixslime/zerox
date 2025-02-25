namespace SixShaded.FourZeroOne.Mellsano;

using System.Diagnostics.CodeAnalysis;

public readonly struct MellsanoID(int id)
{
    public readonly int ID = id;
    public override string ToString() => $"MellsanoID({ID})";
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is MellsanoID other && other.ID == ID;
    public override int GetHashCode() => ID.GetHashCode();
}