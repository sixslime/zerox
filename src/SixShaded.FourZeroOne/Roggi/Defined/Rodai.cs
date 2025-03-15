namespace SixShaded.FourZeroOne.Roggi.Defined;

public sealed class Rodai<R>(Axodu axodu, string identifier) : Axovendu(axodu, identifier), IRoda<R>
    where R : class, Rog
{
    protected override string TypeExpression { get; } = $"Rodai<{typeof(R).Name}>";
}
