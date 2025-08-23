namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas.HexCoordinates;

using Core = Core.Syntax.Core;
using HexCoords = IRoveggi<u.Constructs.uHexCoordinates>;
using HexOffset = IRoveggi<u.Constructs.uHexOffset>;
using HexOffsetType = u.Constructs.uHexOffset;
using HexType = u.Constructs.uHexCoordinates;

public static class Add
{
    public static Korvessa<HexCoords, HexCoords, HexOffset> Construct(IKorssa<HexCoords> a, IKorssa<HexCoords> b) =>
        new(a, b)
        {
            Du = Axoi.Korvedu("HexCoordinates.Add"),
            Definition =
                (_, iA, iB) =>
                    Core.kCompose<HexOffsetType>()
                        .kWithRovi(
                        Core.Hint<HexOffsetType>(),
                        HexType.R,
                        iA.kRef()
                            .kGetRovi(HexType.R)
                            .kAdd(iB.kRef().kGetRovi(HexType.R)))
                        .kWithRovi(
                        Core.Hint<HexOffsetType>(),
                        HexType.U,
                        iA.kRef()
                            .kGetRovi(HexType.U)
                            .kAdd(iB.kRef().kGetRovi(HexType.U)))
                        .kWithRovi(
                        Core.Hint<HexOffsetType>(),
                        HexType.D,
                        iA.kRef()
                            .kGetRovi(HexType.D)
                            .kAdd(iB.kRef().kGetRovi(HexType.D))),
        };
}