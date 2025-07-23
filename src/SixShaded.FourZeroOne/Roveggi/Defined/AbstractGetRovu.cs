namespace SixShaded.FourZeroOne.Roveggi.Defined;

public sealed class AbstractGetRovu<C, R>(Axodu axodu, string identifier) : Axovendu(axodu, identifier), IGetRovu<C, R>, Unsafe.IAbstractRovuTag
    where C : IRovetu
    where R : class, Rog
{
    protected override string TypeExpression => $"AbstractGetRovu<{typeof(C).Name}>";
    public override string ToString() => $"{Identifier}";
}