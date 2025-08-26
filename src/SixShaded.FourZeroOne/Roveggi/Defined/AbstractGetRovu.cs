namespace SixShaded.FourZeroOne.Roveggi.Defined;

public sealed class AbstractGetRovu<C, R>(string identifier) : IGetRovu<C, R>, Unsafe.IAbstractRovu
    where C : IRovetu
    where R : class, Rog
{
    public string Identifier { get; } = identifier;
    public override string ToString() => $"{Identifier}";
}