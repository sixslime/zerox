namespace SixShaded.FourZeroOne.Roveggi.Defined;

using Roveggi;

public sealed class Varovu<C, RKey, RVal>(Axodu axodu, string identifier) : Axovendu(axodu, identifier), IVarovu<C, RKey, RVal>
    where C : IRovetu
    where RKey : class, Rog
    where RVal : class, Rog
{
    protected override string TypeExpression => $"Varovu<{typeof(C).Name}>";
    public IRovu<C, RVal> GenerateRovu(RKey keyRoggi) => new VarovaWrapper<C, RKey, RVal>(this, keyRoggi);
    public override string ToString() => $"{Identifier}";
}