namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas.HexCoordinates;

using Core = Core.Syntax.Core;
using HexCoords = IRoveggi<u.Constructs.uHexCoordinates>;
using HexOffset = IRoveggi<u.Constructs.uHexOffset>;
using HexOffsetType = u.Constructs.uHexOffset;
using HexType = u.Constructs.uHexCoordinates;

public static class GetAdjacent
{
    public static Korvessa<HexCoords, Multi<HexOffset>> Construct(IKorssa<HexCoords> coords) =>
        new(coords)
        {
            Du = Axoi.Korvedu("HexCoordinates.GetAdjacent"),
            Definition =
                (_, iCoords) =>
                    (1, -1, 0).kAsHex()
                    .kGenerateSequence(
                    (iPrevHex, iIndex) =>
                        iIndex.kRef()
                            .kIsGreaterThan(6.kFixed())
                            .kIfTrue<HexOffset>(new()
                            {
                                Then = Core.kNollaFor<HexOffset>(),
                                Else =
                                    iPrevHex.kRef()
                                        .kRotateAround((0, 0, 0).kAsHex(), 1.kFixed())
                            }))
                    .kMap(
                    iHex =>
                        iHex.kRef()
                            .kAdd(iCoords.kRef()))

        };
}