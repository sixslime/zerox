namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.Resolved;

using Core = Core.Syntax.Core;

public interface uResolvedSourcedAbility : uResolved, IConcreteRovetu
{
    public static readonly Rovu<uResolvedSourcedAbility, IRoveggi<Ability.uSourcedAbility>> ABILITY = new(Axoi.Du, "ability");
    public static readonly Rovu<uResolvedSourcedAbility, IRoveggi<Identifier.uUnitIdentifier>> SOURCE = new(Axoi.Du, "source");
    public static readonly Rovu<uResolvedSourcedAbility, IRoveggi<Identifier.uUnitIdentifier>> TARGET = new(Axoi.Du, "target");
}