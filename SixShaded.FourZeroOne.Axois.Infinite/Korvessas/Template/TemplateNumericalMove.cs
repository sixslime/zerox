namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas.Template;

using u.Constructs.Move;
using u.Constructs.Resolved;
using u.Identifier;
using u.Constructs;
using u.Data;
using Core = Core.Syntax.Core;
using u.Constructs.HexTypes;
using Infinite = Syntax.Infinite;
using Obj = IRoveggi<u.Constructs.Move.uNumericalMove>;
using Type = u.Constructs.Move.uNumericalMove;

public record TemplateNumericalMove() : Korvessa<Obj>()
{
    private static readonly CoreStructure.Hint<Type> HINT = new();
    protected override RecursiveMetaDefinition<Obj> InternalDefinition() =>
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
                Type.PATH_SELECTOR,
                Core.kMetaFunction<IRoveggi<uSpaceChecks>, MetaFunction<IRoveggi<uHexIdentifier>, IRoveggi<uHexIdentifier>, Bool>>(
                [],
                iChecks =>
                    Core.kMetaFunction<IRoveggi<uHexIdentifier>, IRoveggi<uHexIdentifier>, Bool>(
                    [iChecks],
                    (iPrev, iStep) =>
                        iChecks.kRef()
                            .kGetRovi(uCheckable.PASSED))))
                .kWithRovi(
                HINT,
                uMove.SUBJECT_COLLECTOR,
                Core.kNollaFor<MetaFunction<IMulti<IRoveggi<uUnitIdentifier>>>>())
                .kWithRovi(
                HINT,
                Type.DISTANCE,
                Core.kNollaFor<NumRange>());
}
