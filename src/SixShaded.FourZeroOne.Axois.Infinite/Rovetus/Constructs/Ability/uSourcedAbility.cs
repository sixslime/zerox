namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.Ability;

using Identifier;

public interface uSourcedAbility : IConcreteRovetu, uAbility
{
    public static new readonly Rovu<uSourcedAbility, IRoveggi<Types.uSourcedType>> TYPE = new("type");
    /// <summary>
    ///     (targetChecks) -> ((source, target) -> isValidTarget) <br></br>
    ///     default should be (targetChecks) => ((_, _) => targetChecks.AllTrue)
    /// </summary>
    public static readonly Rovu<uSourcedAbility, MetaFunction<IRoveggi<uTargetChecks>, MetaFunction<IRoveggi<uUnitIdentifier>, IRoveggi<uUnitIdentifier>, Bool>>> TARGET_SELECTOR = new("target_selector");
    /// <summary>
    ///     (sourceChecks) -> ((unit) -> isValidSource) <br></br>
    ///     default should be (sourceChecks) => ((_) => sourceChecks.AllTrue)
    /// </summary>
    public static readonly Rovu<uSourcedAbility, MetaFunction<IRoveggi<uSourceChecks>, MetaFunction<IRoveggi<uUnitIdentifier>, Bool>>> SOURCE_SELECTOR = new("source_selector");
    public static readonly Rovu<uSourcedAbility, IMulti<IRoveggi<uHexOffset>>> HIT_AREA = new("hit_area");
    public static readonly Rovu<uSourcedAbility, IMulti<IRoveggi<EffectTypes.uUnitEffectType>>> EFFECTS = new("effects");
    /// <summary>
    ///     (source, target) -> action
    /// </summary>
    public static readonly Rovu<uSourcedAbility, MetaFunction<IRoveggi<uUnitIdentifier>, IRoveggi<uUnitIdentifier>, Rog>> FOLLOWUP = new("followup");
    public static readonly ImplementationStatement<uSourcedAbility> __IMPLEMENTS = c => c.ImplementGet(uAbility.TYPE, iSelf => iSelf.kRef().kGetRovi(TYPE));
}