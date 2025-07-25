namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Config;

using Constructs.Ability;

public interface uPlayerDeclaration : IConcreteRovetu
{
    public static readonly Rovu<uPlayerDeclaration, IMulti<IRoveggi<uAbility>>> DECK = new(Axoi.Du, "deck");
    public static readonly Rovu<uPlayerDeclaration, Number> HAND_SIZE = new(Axoi.Du, "hand_size");
    public static readonly Rovu<uPlayerDeclaration, IMulti<MetaFunction<IRoveggi<Identifier.uPlayerIdentifier>, Rog>>> MODIFIERS = new(Axoi.Du, "modifiers");
}