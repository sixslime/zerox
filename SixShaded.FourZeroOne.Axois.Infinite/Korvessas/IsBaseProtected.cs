namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas;

using u.Constructs.Move;
using u.Constructs.Resolved;
using u.Identifier;
using u.Data;
using Core = Core.Syntax.Core;
using u.Constructs.HexTypes;
using Infinite = Syntax.Infinite;

public static class IsBaseProtected
{
    public static Korvessa<IRoveggi<uPlayerIdentifier>, Bool> Construct(IKorssa<IRoveggi<uPlayerIdentifier>> player) =>
        new(player)
        {
            Du = Axoi.Korvedu("IsBaseProtected"),
            Definition =
                (_, iPlayer) =>
                    Infinite.AllUnits
                        .kAnyMatch(
                        iUnit =>
                            iUnit.kRef()
                                .kRead()
                                .kGetRovi(uUnitData.OWNER)
                                .kEquals(iPlayer.kRef())
                                .ksLazyAnd(
                                iUnit.kRef()
                                    .kRead()
                                    .kGetRovi(uUnitData.POSITION)
                                    .kRead()
                                    .kGetRovi(uHexData.TYPE)
                                    .kIsType<uBaseHex>()
                                    .kGetRovi(uBaseHex.OWNER)
                                    .kEquals(iPlayer.kRef())
                                    .kCatchNolla(
                                    () =>
                                        false.kFixed()))),
        };
}