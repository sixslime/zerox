namespace SixShaded.FourZeroOne.Roveggi.Defined;

public sealed class AbstractSetRovu<C, R>(string identifier) : ISetRovu<C, R>, Unsafe.IAbstractRovu
    where C : IRovetu
    where R : class, Rog
{
    public string Identifier { get; } = identifier;
    public override string ToString() => $"{Identifier}";
}