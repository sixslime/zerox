namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas.HexCoordinates;

using Core = Core.Syntax.Core;
using HexCoords = IRoveggi<u.Constructs.uHexCoordinates>;
using HexOffset = IRoveggi<u.Constructs.uHexOffset>;
using HexOffsetType = u.Constructs.uHexOffset;
using HexType = u.Constructs.uHexCoordinates;

public record RotateAround(IKorssa<HexCoords> coordinate, IKorssa<HexCoords> anchor, IKorssa<Number> rotation) : Korvessa<HexCoords, HexCoords, Number, HexOffset>(coordinate, anchor, rotation)
{
    private static IKorssa<HexOffset> _zero => (0, 0, 0).kAsHex();

    protected override RecursiveMetaDefinition<HexCoords, HexCoords, Number, HexOffset> InternalDefinition() =>
        (_, iCoordinate, iAnchor, iRotation) =>
            Core.kMetaFunctionRecursive<HexOffset, Number, HexOffset>(
                [], (iRecurse, iHex, iRotationsLeft) =>
                    iRotationsLeft.kRef()
                        .kIsGreaterThan(0.kFixed())
                        .kIfTrue<HexOffset>(
                        new()
                        {
                            Then =
                                iRecurse.kRef()
                                    .kExecuteWith(
                                    new()
                                    {
                                        A =
                                            _zero.kSubtract(
                                            Core.kCompose<HexOffsetType>()
                                                .kWithRovi(
                                                Core.Hint<HexOffsetType>(),
                                                HexType.R,
                                                iHex.kRef()
                                                    .kGetRovi(HexType.U))
                                                .kWithRovi(
                                                Core.Hint<HexOffsetType>(),
                                                HexType.U,
                                                iHex.kRef()
                                                    .kGetRovi(HexType.D))
                                                .kWithRovi(
                                                Core.Hint<HexOffsetType>(),
                                                HexType.D,
                                                iHex.kRef()
                                                    .kGetRovi(HexType.R))),
                                        B = iRotationsLeft.kRef().kSubtract(1.kFixed()),
                                    }),
                            Else = iHex.kRef(),
                        }))
                .kExecuteWith(
                new()
                {
                    A =
                        iCoordinate.kRef()
                            .kSubtract(iAnchor.kRef()),
                    B =
                        iRotation.kRef()
                            .kModulo(6.kFixed()),
                })
                .kAdd(iAnchor.kRef());
}
