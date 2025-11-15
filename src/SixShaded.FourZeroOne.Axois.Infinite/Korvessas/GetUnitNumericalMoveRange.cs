namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas;

using Rovetus.Constructs.EffectTypes;
using u.Constructs.Ability;
using u.Constructs.Resolved;
using Core = Core.Syntax.Core;
using u.Constructs.Ability.Types;
using u.Identifier;
using u.Data;
using Infinite = Syntax.Infinite;
using u.Constructs;

public record GetUnitNumericalMoveRange(IKorssa<IRoveggi<uUnitIdentifier>> unit, IKorssa<NumRange> moveRange) : Korvessa<IRoveggi<uUnitIdentifier>, NumRange, NumRange>(unit, moveRange)
{
    protected override RecursiveMetaDefinition<IRoveggi<uUnitIdentifier>, NumRange, NumRange> InternalDefinition() =>
        (_, iUnit, iMoveRange) =>
            Core.kSubEnvironment<NumRange>(
            new()
            {
                Environment =
                [
                    iUnit.kRef()
                        .kRead()
                        .kGetRovi(uUnitData.EFFECTS)
                        .kAnyMatch(iEffect => iEffect.kRef().kGetRovi(uUnitEffect.TYPE).kIsType<uSlowEffect>().kExists())
                        .kIfTrue<Number>(
                        new()
                        {
                            Then = 2.kFixed(),
                            Else = 1.kFixed(),
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
                            .kDivide(iThisSlowFactor.kRef())),
            });
}
