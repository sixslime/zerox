namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas;

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
                    Core.kCompose<uSubjectChecks>()
                        .kWithRovi(uSubjectChecks.EFFECT_CHECK,
                        iUnit.kRef()
                            .kRead()
                            .kGetRovi(uUnitData.EFFECTS)
                            .kContains(Core.kCompose<u.Constructs.UnitEffects.uImmobileEffect>())
                            .kNot())
        };
}