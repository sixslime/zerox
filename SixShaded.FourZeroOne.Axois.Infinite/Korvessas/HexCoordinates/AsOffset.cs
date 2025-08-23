namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas.HexCoordinates;

using Core = Core.Syntax.Core;
using HexCoords = IRoveggi<u.Constructs.uHexCoordinates>;
using HexOffset = IRoveggi<u.Constructs.uHexOffset>;
using HexOffsetType = u.Constructs.uHexOffset;
using HexType = u.Constructs.uHexCoordinates;

public static class AsOffset
{
    public static Korvessa<HexCoords, HexOffset> Construct(IKorssa<HexCoords> coords) =>
        new(coords)
        {
            Du = Axoi.Korvedu("HexCoordinates.AsOffset"),
            Definition =
                (_, iCoords) =>
                    iCoords.kRef()
                        .kIsType<HexOffset>()
                        .kCatchNolla(
                        () =>
                            Core.kCompose<HexOffsetType>()
                                .kWithRovi(
                                Core.Hint<HexOffsetType>(),
                                HexType.R,
                                iCoords.kRef()
                                    .kGetRovi(HexType.R))
                                .kWithRovi(
                                Core.Hint<HexOffsetType>(),
                                HexType.U,
                                iCoords.kRef()
                                    .kGetRovi(HexType.U))
                                .kWithRovi(
                                Core.Hint<HexOffsetType>(),
                                HexType.D,
                                iCoords.kRef()
                                    .kGetRovi(HexType.D))),
        };
}