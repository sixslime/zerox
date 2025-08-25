namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas;

using u.Constructs.Move;
using u.Constructs.Resolved;
using u.Constructs.HexTypes;
using u.Identifier;
using u.Data;
using Core = Core.Syntax.Core;
using Infinite = Syntax.Infinite;

public record CheckMoveDestination(IKorssa<IRoveggi<uHexIdentifier>> hex, IKorssa<IRoveggi<uUnitIdentifier>> unit) : Korvessa<IRoveggi<uHexIdentifier>, IRoveggi<uUnitIdentifier>, IRoveggi<uSpaceChecks>>(hex, unit)
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
                    iHex.kRef()
                        .kRead()
                        .kAsVariable(out var iHexData),
                ],
                Value =
                    Core.kCompose<uSpaceChecks>()
                        .kWithRovi(
                        uSpaceChecks.WALL,
                        iHexData.kRef()
                            .kGetRovi(uHexData.TYPE)
                            .kIsType<uWallHex>()
                            .kExists())
                        .kWithRovi(
                        uSpaceChecks.PROTECTED_BASE,
                        Core.kSubEnvironment<Bool>(
                        new()
                        {
                            Environment =
                            [
                                iHexData.kRef()
                                    .kGetRovi(uHexData.TYPE)
                                    .kIsType<uBaseHex>()
                                    .kGetRovi(uBaseHex.OWNER)
                                    .kAsVariable(out var iBaseOwner),
                            ],
                            Value =
                                iBaseOwner.kRef()
                                    .kEquals(
                                    iSubjectData.kRef()
                                        .kGetRovi(uUnitData.OWNER))
                                    .ksLazyOr(
                                    iBaseOwner.kRef()
                                        .kIsBaseProtected()
                                        .kNot())
                                    .kCatchNolla(() => true.kFixed()),
                        }))
                        .kWithRovi(
                        uSpaceChecks.UNIT,
                        Infinite.AllUnits.kFirstMatch(
                        iUnit =>
                            iUnit.kRef()
                                .kRead()
                                .kGetRovi(uUnitData.POSITION)
                                .kEquals(iHex.kRef()))),
            });
}
