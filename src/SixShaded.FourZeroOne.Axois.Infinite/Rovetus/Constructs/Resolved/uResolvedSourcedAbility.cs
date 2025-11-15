namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.Resolved;

using Core = Core.Syntax.Core;

public interface uResolvedSourcedAbility : IConcreteRovetu, uResolvedAbility
{
    public static new readonly Rovu<uResolvedSourcedAbility, IRoveggi<Ability.uSourcedAbility>> ABILITY = new("ability");
    public static readonly Rovu<uResolvedSourcedAbility, IRoveggi<Identifier.uUnitIdentifier>> SOURCE = new("source");
    public static readonly Rovu<uResolvedSourcedAbility, IRoveggi<Identifier.uUnitIdentifier>> TARGET = new("target");
    public static readonly ImplementationStatement<uResolvedSourcedAbility> __IMPLEMENTS =
        c => c.ImplementGet(uResolvedAbility.ABILITY, iSelf => iSelf.kRef().kGetRovi(ABILITY));
}