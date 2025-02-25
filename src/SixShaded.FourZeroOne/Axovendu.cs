namespace SixShaded.FourZeroOne;

public abstract class Axovendu(Axodu axodu, string identifier) : IAxovendu
{
    protected abstract string TypeExpression { get; }
    public Axodu Axodu { get; } = axodu;
    public string Identifier { get; } = identifier.ToLower();
    string IAxovendu.TypeExpression => TypeExpression;

    public override bool Equals(object? obj) => obj is Axovendu other && other.Axodu == Axodu && other.Identifier == Identifier && other.TypeExpression == TypeExpression;
    public override int GetHashCode() => HashCode.Combine(Axodu, Identifier, TypeExpression);
}