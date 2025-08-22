namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas;

using Rovetus.Constructs.EffectTypes;
using u.Constructs.Move;
using u.Constructs.Resolved;
using u.Identifier;
using u.Data;
using Core = Core.Syntax.Core;

public static class DoMoveSubjectChecks
{
    public static Korvessa<IRoveggi<uMove>, IRoveggi<uUnitIdentifier>, IRoveggi<uSubjectChecks>> Construct(IKorssa<IRoveggi<uMove>> move, IKorssa<IRoveggi<uUnitIdentifier>> unit) =>
        new(move, unit)
        {
            Du = Axoi.Korvedu("DoMoveSubjectChecks"),
            Definition =
                (_, iMove, iUnit) =>
                    Core.kSubEnvironment<IRoveggi<uSubjectChecks>>(new()
                    {
                        Environment =
                            [
                                iUnit.kRef()
                                    .kRead()
                                    .kAsVariable(out var iData)
                            ],
                        Value =
                            Core.kCompose<uSubjectChecks>()
                                .kWithRovi(uSubjectChecks.EFFECT_CHECK,
                                iData.kRef()
                                    .kGetRovi(uUnitData.EFFECTS)
                                    .kAnyMatch(iEffect => iEffect.kRef().kIsType<uImmobileEffect>().kExists())
                                    .kNot())
                    })
        };
}