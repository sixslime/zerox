namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.Resolved;

using Core = Core.Syntax.Core;

public interface uResolvedSourcedAbility : uResolved, IConcreteRovetu
{
    public static readonly Rovu<uResolvedSourcedAbility, IRoveggi<Ability.uSourcedAbility>> ABILITY = new(Axoi.Du, "ability");
    public static readonly Rovu<uResolvedSourcedAbility, IRoveggi<Identifier.uUnitIdentifier>> SOURCE = new(Axoi.Du, "source");
    public static readonly Rovu<uResolvedSourcedAbility, IRoveggi<Identifier.uUnitIdentifier>> TARGET = new(Axoi.Du, "target");
    public static readonly ImplementationStatement<uResolvedSourcedAbility> __IMPLEMENTS =
        c =>
            c.ImplementGet(
            INSTRUCTIONS,
            iSelf =>
                Core.kSubEnvironment<Rog>(
                new()
                {
                    Environment = [iSelf.kRef().kGetRovi(ABILITY).kAsVariable(out var iAbility), iSelf.kRef().kGetRovi(SOURCE).kAsVariable(out var iSource), iSelf.kRef().kGetRovi(TARGET).kAsVariable(out var iTarget)],
                    Value =
                        Core.kMulti<Rog>(
                        iTarget.kRef()
                            .kUpdate(
                            iTargetData =>
                                iTargetData.kRef()
                                    .kUpdateRovi(
                                    Data.uUnitData.EFFECTS,
                                    iEffects =>
                                        iEffects.kRef()
                                            .kConcat(iAbility.kRef().kGetRovi(Ability.uSourcedAbility.EFFECTS)))),
                        iAbility.kRef()
                            .kGetRovi(Ability.uSourcedAbility.FOLLOWUP)
                            .kExecuteWith(
                            new()
                            {
                                A = iSource.kRef(),
                                B = iTarget.kRef(),
                            })),
                }));
}           