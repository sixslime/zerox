namespace SixShaded.FourZeroOne.Roggi.Defined;

public sealed class Rovu<C, R>(Axodu axodu, string identifier) : Axovendu(axodu, identifier), IRovu<C, R> where C : IRoveggitu where R : class, Rog
{
    protected override string TypeExpression => $"Rovu<{typeof(C).Name}>";
}