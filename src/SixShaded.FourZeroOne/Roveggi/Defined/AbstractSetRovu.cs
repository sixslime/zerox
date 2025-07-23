namespace SixShaded.FourZeroOne.Roveggi.Defined;

public sealed class AbstractSetRovu<C, R>(Axodu axodu, string identifier) : Axovendu(axodu, identifier), ISetRovu<C, R>, Unsafe.IAbstractRovu
    where C : IRovetu
    where R : class, Rog
{
    protected override string TypeExpression => $"AbstractSetRovu<{typeof(C).Name}>";
    public override string ToString() => $"{Identifier}";
}