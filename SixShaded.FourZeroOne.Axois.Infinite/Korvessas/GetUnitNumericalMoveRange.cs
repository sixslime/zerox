namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas;

using Rovetus.Constructs.EffectTypes;
using u.Constructs.Ability;
using u.Constructs.Resolved;
using Core = Core.Syntax.Core;
using u.Constructs.Ability.Types;
using u.Identifier;
using u.Data;
using Infinite = Syntax.Infinite;

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
                            iMoveRange.kRef()
                                .kStart()
                                .kAtLeast(0.kFixed())
                                .kDivide(iThisSlowFactor.kRef())
                                .kRangeTo(
                                iMoveRange.kRef()
                                    .kEnd()
                                    .kDivide(iThisSlowFactor.kRef()))
                    })
        };
}