namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas.HexCoordinates;

using Core = Core.Syntax.Core;
using HexCoords = IRoveggi<u.Constructs.uHexCoordinates>;
using HexOffset = IRoveggi<u.Constructs.uHexOffset>;
using HexOffsetType = u.Constructs.uHexOffset;
using HexType = u.Constructs.uHexCoordinates;

public static class RotateAround
{
    private static IKorssa<HexOffset> _zero => (0, 0, 0).kAsHex();
    // clockwise
    public static Korvessa<HexCoords, HexCoords, Number, HexOffset> Construct(IKorssa<HexCoords> coordinate, IKorssa<HexCoords> anchor, IKorssa<Number> rotation) =>
        new(coordinate, anchor, rotation)
        {
            Du = Axoi.Korvedu("HexCoordinates.RotateAround"),
            Definition =
                (_, iCoordinate, iAnchor, iRotation) =>
                    Core.kSubEnvironment<HexOffset>(new()
                    {
                        Environment =
                            [

                            ],
                        Value = Core.kMetaFunctionRecursive<HexOffset, Number, HexOffset>(
                        [], (iRecurse, iHex, iRotationsLeft) =>
                            iRotationsLeft.kRef()
                                .kIsGreaterThan(0.kFixed())
                                .kIfTrue<HexOffset>(new()
                                {
                                    Then =
                                        iRecurse.kRef()
                                            .kExecuteWith(new()
                                            {
                                                A = _zero.kSubtract(
                                                Core.kCompose<HexOffsetType>()
                                                    .kWithRovi(Core.Hint<HexOffsetType>(),
                                                    HexType.R,
                                                    iHex.kRef()
                                                        .kGetRovi(HexType.U))
                                                    .kWithRovi(Core.Hint<HexOffsetType>(),
                                                    HexType.U,
                                                    iHex.kRef()
                                                        .kGetRovi(HexType.D))
                                                    .kWithRovi(Core.Hint<HexOffsetType>(),
                                                    HexType.D,
                                                    iHex.kRef()
                                                        .kGetRovi(HexType.R)))
                                            })
                                })

                    })
        };
}