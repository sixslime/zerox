namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas;

using u.Constructs.Move;
using u.Constructs.Resolved;
using u.Constructs.HexTypes;
using u.Identifier;
using u.Data;
using Core = Core.Syntax.Core;
using Infinite = Syntax.Infinite;

public record CheckMovePath(IKorssa<IRoveggi<uHexIdentifier>> hex, IKorssa<IRoveggi<uUnitIdentifier>> unit) : Korvessa<IRoveggi<uHexIdentifier>, IRoveggi<uUnitIdentifier>, IRoveggi<uSpaceChecks>>(hex, unit)
{
    protected override RecursiveMetaDefinition<IRoveggi<uHexIdentifier>, IRoveggi<uUnitIdentifier>, IRoveggi<uSpaceChecks>> InternalDefinition() =>
        (_, iHex, iSubject) =>
            Core.kSubEnvironment<IRoveggi<uSpaceChecks>>(
            new()
            {
                Environment =
                [
                    iSubject.kRef()
                        .kRead()
                        .kAsVariable(out var iSubjectData),
                ],
                Value =
                    iHex.kRef()
                        .kMoveDestinationChecks(iSubject.kRef())
                        .kWithRovi(
                        uSpaceChecks.UNIT,
                        Infinite.AllUnits.kFirstMatch(
                        iUnit =>
                            iUnit.kRef()
                                .kRead()
                                .kGetRovi(uUnitData.POSITION)
                                .kEquals(iHex.kRef())
                                .ksLazyAnd(
                                iUnit.kRef()
                                    .kRead()
                                    .kGetRovi(uUnitData.OWNER)
                                    .kEquals(
                                    iSubjectData.kRef()
                                        .kGetRovi(uUnitData.OWNER))
                                    .kNot()))),
            });
}
