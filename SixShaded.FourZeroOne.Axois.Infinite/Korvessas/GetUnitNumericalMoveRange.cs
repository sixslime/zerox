namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas;

using u.Constructs.Ability;
using u.Constructs.Resolved;
using Core = Core.Syntax.Core;
using u.Constructs.Ability.Types;
using u.Identifier;
using u.Data;
using Infinite = Syntax.Infinite;
using u.Constructs.UnitEffects;
public static class GetUnitNumericalMoveRange
{
    public static Korvessa<IRoveggi<uUnitIdentifier>, NumRange, NumRange> Construct(IKorssa<IRoveggi<uUnitIdentifier>> unit, IKorssa<NumRange> moveRange) =>
        new(unit, moveRange)
        {
            Du = Axoi.Korvedu("GetUnitNumericalMoveRange"),
            Definition =
                (_, iUnit, iMoveRange) =>
                    Core.kSubEnvironment<NumRange>(
                    new()
                    {
                        Environment =
                        [
                            iUnit.kRef()
                                .kRead()
                                .kGetRovi(uUnitData.EFFECTS)
                                .kAsVariable(out var iEffects),
                            iEffects.kRef()
                                .kAnyMatch(iEffect => iEffect.kRef().kIsType<uSlowEffect>().kExists())
                                .kIfTrue<Number>(
                                new()
                                {
                                    Then = 2.kFixed(),
                                    Else = 1.kFixed()
                                })
                                .kAsVariable(out var iThisSlowFactor),
                        ],
                        Value =
                            iEffects.kRef()
                                .kAnyMatch(iEffect => iEffect.kRef().kIsType<uImmobileEffect>().kExists())
                                .kIfTrue<NumRange>(
                                new()
                                {
                                    Then = 0.kFixed().kSingleRange(),
                                    Else =
                                        iMoveRange.kRef()
                                            .kStart()
                                            .kAtLeast(0.kFixed())
                                            .kDivide(iThisSlowFactor.kRef())
                                            .kRangeTo(
                                            iMoveRange.kRef()
                                                .kEnd()
                                                .kDivide(iThisSlowFactor.kRef()))
                                })
                    })
        };
}