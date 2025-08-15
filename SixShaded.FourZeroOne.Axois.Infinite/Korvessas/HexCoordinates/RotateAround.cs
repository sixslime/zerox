namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas.HexCoordinates;

using Core = Core.Syntax.Core;
using HexCoords = IRoveggi<u.Constructs.uHexCoordinates>;
using HexOffset = IRoveggi<u.Constructs.uHexOffset>;
using HexOffsetType = u.Constructs.uHexOffset;
using HexType = u.Constructs.uHexCoordinates;

public static class RotateAround
{
    // clockwise
    public static Korvessa<HexCoords, HexCoords, Number, HexCoords> Construct(IKorssa<HexCoords> coordinate, IKorssa<HexCoords> anchor, IKorssa<Number> rotation) =>
        new(coordinate, anchor, rotation)
        {
            Du = Axoi.Korvedu("HexCoordinates.RotateAround"),
            Definition =
                (_, iCoordinate, iAnchor, iRotation) =>
                    Core.kCompose<HexOffsetType>()
                        .kWithRovi(HexType.R, 
                        iA.kRef().kGetRovi(HexType.R)
                            .kAdd(iB.kRef().kGetRovi(HexType.R)))
                        .kWithRovi(HexType.U,
                        iA.kRef().kGetRovi(HexType.U)
                            .kAdd(iB.kRef().kGetRovi(HexType.U)))
                        .kWithRovi(HexType.D,
                        iA.kRef().kGetRovi(HexType.D)
                            .kAdd(iB.kRef().kGetRovi(HexType.D)))
        };
}