namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Config;

using Constructs.Ability;

public interface uPlayerDeclaration : IConcreteRovetu
{
    public static readonly Rovu<uPlayerDeclaration, IMulti<IRoveggi<uAbility>>> DECK = new("deck");
    public static readonly Rovu<uPlayerDeclaration, Number> HAND_SIZE = new("hand_size");
    public static readonly Rovu<uPlayerDeclaration, IMulti<MetaFunction<IRoveggi<Identifier.uPlayerIdentifier>, Rog>>> MODIFIERS = new("modifiers");
    public static readonly Rovu<uPlayerDeclaration, Number> PERSPECTIVE_ROTATION = new("perspective_rotation");
}