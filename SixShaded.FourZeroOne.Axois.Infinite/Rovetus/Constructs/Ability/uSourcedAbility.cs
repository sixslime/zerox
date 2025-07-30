namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.Ability;

using Identifier;

public interface uSourcedAbility : IConcreteRovetu, uAbility
{
    public static new readonly Rovu<uSourcedAbility, IRoveggi<Types.uSourcedType>> TYPE = new(Axoi.Du, "type");
    public static readonly Rovu<uSourcedAbility, IMulti<IRoveggi<uRelativeCoordinate>>> HIT_AREA = new(Axoi.Du, "hit_area");
    public static readonly Rovu<uSourcedAbility, IMulti<IRoveggi<UnitEffects.uUnitEffect>>> EFFECTS = new(Axoi.Du, "effects");
    /// <summary>
    /// (source, target) -> action
    /// </summary>
    public static readonly Rovu<uSourcedAbility, MetaFunction<IRoveggi<uUnitIdentifier>, IRoveggi<uUnitIdentifier>, Rog>> FOLLOWUP = new(Axoi.Du, "followup");

    public static readonly ImplementationStatement<uSourcedAbility> __IMPLEMENTS = c => c.ImplementGet(uAbility.TYPE, iSelf => iSelf.kRef().kGetRovi(TYPE));
}