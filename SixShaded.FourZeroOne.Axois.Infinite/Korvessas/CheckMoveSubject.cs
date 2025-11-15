namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas;

using Rovetus.Constructs.EffectTypes;
using u.Constructs.Move;
using u.Constructs.Resolved;
using u.Identifier;
using u.Data;
using Core = Core.Syntax.Core;
using u.Constructs;

public record CheckMoveSubject(IKorssa<IRoveggi<uMove>> move, IKorssa<IRoveggi<uUnitIdentifier>> unit) : Korvessa<IRoveggi<uMove>, IRoveggi<uUnitIdentifier>, IRoveggi<uSubjectChecks>>(move, unit)
{
    protected override RecursiveMetaDefinition<IRoveggi<uMove>, IRoveggi<uUnitIdentifier>, IRoveggi<uSubjectChecks>> InternalDefinition() =>
        (_, iMove, iUnit) =>
            Core.kSubEnvironment<IRoveggi<uSubjectChecks>>(
            new()
            {
                Environment =
                [
                    iUnit.kRef()
                        .kRead()
                        .kAsVariable(out var iData),
                ],
                Value =
                    Core.kCompose<uSubjectChecks>()
                        .kWithRovi(
                        uSubjectChecks.EFFECT_CHECK,
                        iData.kRef()
                            .kGetRovi(uUnitData.EFFECTS)
                            .kAnyMatch(iEffect => iEffect.kRef().kGetRovi(uUnitEffect.TYPE).kIsType<uImmobileEffect>().kExists())
                            .kNot()),
            });
}
