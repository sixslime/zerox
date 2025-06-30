namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.Ability;

using Identifier;

public interface uSourcedAbility : IRovetu, uAbility
{
    public static readonly Rovu<uSourcedAbility, IRoveggi<Types.uSourcedType>> TYPE = new(Axoi.Du, "type");
    public static readonly Rovu<uSourcedAbility, IMulti<IRoveggi<uRelativeCoordinate>>> HIT_AREA = new(Axoi.Du, "hit_area");
    /// <summary>
    /// Inputs are source unit and the available targets within the hit area (accounting for attack/defense).<br></br>
    /// Output is the final list of valid targets.
    /// </summary>
    public static readonly Rovu<uSourcedAbility, MetaFunction<IRoveggi<uUnitIdentifier>, IMulti<IRoveggi<uUnitIdentifier>>, IMulti<IRoveggi<uUnitIdentifier>>>> POST_TARGETING = new(Axoi.Du, "post_targeting");
    public static readonly Rovu<uSourcedAbility, IMulti<IRoveggi<UnitEffects.uUnitEffect>>> EFFECTS = new(Axoi.Du, "effects");
    public static readonly Rovu<uSourcedAbility, MetaFunction<IRoveggi<uUnitIdentifier>, IRoveggi<uUnitIdentifier>, Rog>> FOLLOWUP = new(Axoi.Du, "followup");
}