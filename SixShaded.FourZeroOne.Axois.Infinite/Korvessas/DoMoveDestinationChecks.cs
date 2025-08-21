namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas;

using u.Constructs.Move;
using u.Constructs.Resolved;
using u.Constructs.HexTypes;
using u.Identifier;
using u.Data;
using Core = Core.Syntax.Core;
using Infinite = Syntax.Infinite;

public static class DoMoveDestinationChecks
{
    public static Korvessa<IRoveggi<uHexIdentifier>, IRoveggi<uUnitIdentifier>, IRoveggi<uSpaceChecks>> Construct(IKorssa<IRoveggi<uHexIdentifier>> hex, IKorssa<IRoveggi<uUnitIdentifier>> unit) =>
        new(hex, unit)
        {
            Du = Axoi.Korvedu("DoMoveDestinationChecks"),
            Definition =
                (_, iHex, iSubject) =>
                    Core.kCompose<uSpaceChecks>()
                        .kWithRovi(
                        uSpaceChecks.WALL,
                        iHex.kRef()
                            .kRead()
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
                                iHex.kRef()
                                    .kRead()
                                    .kGetRovi(uHexData.TYPE)
                                    .kIsType<uBaseHex>()
                                    .kGetRovi(uBaseHex.OWNER)
                                    .kAsVariable(out var iBaseOwner),
                            ],
                            Value =
                                iBaseOwner.kRef()
                                    .kEquals(
                                    iSubject.kRef()
                                        .kRead()
                                        .kGetRovi(uUnitData.OWNER))
                                    .ksLazyOr(
                                    iBaseOwner.kRef()
                                        .kIsBaseProtected()
                                        .kNot())
                                    .kCatchNolla(() => true.kFixed())
                        }))
                        .kWithRovi(
                        uSpaceChecks.UNIT,
                        Infinite.AllUnits.kFirstMatch(
                        iUnit =>
                            iUnit.kRef()
                                .kRead()
                                .kGetRovi(uUnitData.POSITION)
                                .kEquals(iHex.kRef())))
        };
}