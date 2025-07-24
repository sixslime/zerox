namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.Ability;

public interface uUnsourcedAbility : IConcreteRovetu, uAbility
{
    public static readonly Rovu<uUnsourcedAbility, IRoveggi<Types.uUnsourcedType>> TYPE = new(Axoi.Du, "type");
    public static readonly Rovu<uUnsourcedAbility, MetaFunction<Rog>> ACTION = new(Axoi.Du, "action");

}