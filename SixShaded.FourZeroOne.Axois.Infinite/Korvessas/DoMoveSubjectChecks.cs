namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas;

using u.Constructs.Move;
using u.Constructs.Resolved;
using u.Constructs.HexTypes;
using u.Identifier;
using u.Data;
using Core = Core.Syntax.Core;

public static class DoMoveSpaceChecks
{
    public static Korvessa<IRoveggi<uUnitIdentifier>, IRoveggi<uHexIdentifier>, IRoveggi<uSpaceChecks>> Construct(IKorssa<IRoveggi<uUnitIdentifier>> unit, IKorssa<IRoveggi<uHexIdentifier>> hex) =>
        new(unit, hex)
        {
            Du = Axoi.Korvedu("DoMoveSpaceChecks"),
            Definition =
                (_, iUnit, iHex) =>
                    Core.kCompose<uSpaceChecks>()
                        .kWithRovi(uSpaceChecks.WALL,
                        iHex.kRef()
                            .kRead()
                            .kGetRovi(uHexData.TYPE)
                            .kIsType<uWallHex>()
                            .kExists())
                        .kWithRovi(uSpaceChecks.PROTECTED_BASE,
                        iHex.kRef()
                            .kRead()
                            .kGetRovi(uHexData.TYPE)
                            .kIsType<uBaseHex>()
                            .kGetRovi(uBaseHex.OWNER)
                            .kEquals(
                            iUnit.kRef()
                                .kRead()
                                .kGetRovi(uUnitData.OWNER))
                            .kNot()
                            .kCatchNolla(() => true.kFixed()))
        };
}