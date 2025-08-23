namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas.Template;

using u.Constructs.Move;
using u.Constructs.Resolved;
using u.Identifier;
using u.Constructs;
using u.Data;
using Core = Core.Syntax.Core;
using u.Constructs.HexTypes;
using Infinite = Syntax.Infinite;
using Obj = IRoveggi<u.Constructs.Move.uPositionalMove>;
using Type = u.Constructs.Move.uPositionalMove;

public static class TemplatePositionalMove
{
    private static readonly CoreStructure.Hint<Type> HINT = new();

    public static Korvessa<Obj> Construct() =>
        new()
        {
            Du = Axoi.Korvedu("TemplatePositionalMove"),
            Definition =
                _ =>
                    Core.kCompose<Type>()
                        .kWithRovi(
                        HINT,
                        uMove.ENVIRONMENT_PREMOD, Core.kNollaFor<Rog>().kMetaBoxed([]))
                        .kWithRovi(
                        HINT,
                        uMove.SUBJECT_SELECTOR,
                        Core.kMetaFunction<IRoveggi<uSubjectChecks>, MetaFunction<IRoveggi<uUnitIdentifier>, Bool>>(
                        [],
                        iChecks =>
                            Core.kMetaFunction<IRoveggi<uUnitIdentifier>, Bool>(
                            [iChecks],
                            iUnit =>
                                iChecks.kRef()
                                    .kGetRovi(uCheckable.PASSED))))
                        .kWithRovi(
                        HINT,
                        uMove.DESTINATION_SELECTOR,
                        Core.kMetaFunction<IRoveggi<uSpaceChecks>, MetaFunction<IRoveggi<uHexIdentifier>, Bool>>(
                        [],
                        iChecks =>
                            Core.kMetaFunction<IRoveggi<uHexIdentifier>, Bool>(
                            [iChecks],
                            iUnit =>
                                iChecks.kRef()
                                    .kGetRovi(uCheckable.PASSED))))
                        .kWithRovi(
                        HINT,
                        uMove.SUBJECT_COLLECTOR,
                        Core.kNollaFor<MetaFunction<IMulti<IRoveggi<uUnitIdentifier>>>>())
                        .kWithRovi(
                        HINT,
                        Type.MOVE_FUNCTION,
                        Core.kNollaFor<MetaFunction<IRoveggi<uUnitIdentifier>, IMulti<IRoveggi<uHexIdentifier>>>>()),
        };
}