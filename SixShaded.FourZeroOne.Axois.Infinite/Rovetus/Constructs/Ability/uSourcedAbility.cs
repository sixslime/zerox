namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.Ability;

public interface uSourcedAbility : IRovetu, uAbility
{
    public static readonly Rovu<uSourcedAbility, IRoveggi<Types.uSourcedType>> TYPE = new(Axoi.Du, "type");
    public static readonly Rovu<uSourcedAbility, IMulti<IRoveggi<uRelativeCoordinate>>> HIT_AREA = new(Axoi.Du, "hit_area");
    public static readonly Rovu<uSourcedAbility, MetaFunction<IRoveggi<Identifiers.uUnitIdentifier>, IMulti<IRoveggi<Identifiers.uUnitIdentifier>>, IMulti<IRoveggi<Identifiers.uUnitIdentifier>>>> ADDITIONAL_TARGETING = new(Axoi.Du, "additional_targeting");
    public static readonly Rovu<uSourcedAbility, IMulti<IRoveggi<UnitEffects.uUnitEffect>>> EFFECTS = new(Axoi.Du, "effects");
    public static readonly Rovu<uSourcedAbility, MetaFunction<IRoveggi<Identifiers.uUnitIdentifier>, IRoveggi<Identifiers.uUnitIdentifier>, Rog>> FOLLOWUP = new(Axoi.Du, "followup");
}