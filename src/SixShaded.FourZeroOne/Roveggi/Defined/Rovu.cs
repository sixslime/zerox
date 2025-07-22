namespace SixShaded.FourZeroOne.Roveggi.Defined;

using Roveggi;

public sealed class Rovu<C, R>(Axodu axodu, string identifier) : Axovendu(axodu, identifier), IRovu<C, R>
    where C : IRovetu
    where R : class, Rog
{
    protected override string TypeExpression => $"Rovu<{typeof(C).Name}>";
    public override string ToString() => $"{Identifier}";
}