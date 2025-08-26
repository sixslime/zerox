namespace SixShaded.FourZeroOne.Roveggi.Defined;

using Roveggi;

public sealed class Rovu<C, R>(string identifier) : IRovu<C, R>
    where C : IRovetu
    where R : class, Rog
{
    public string Identifier { get; } = identifier;
    public override string ToString() => $"{Identifier}";
}