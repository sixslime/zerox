namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas.HexCoordinates;

using Core = Core.Syntax.Core;
using HexCoords = IRoveggi<u.Constructs.uHexCoordinates>;
using HexIdentifier = IRoveggi<u.Identifier.uHexIdentifier>;
using HexIdentifierType = u.Identifier.uHexIdentifier;
using HexType = u.Constructs.uHexCoordinates;

public record AsAbsolute(IKorssa<HexCoords> coords) : Korvessa<HexCoords, HexIdentifier>(coords)
{
    protected override RecursiveMetaDefinition<HexCoords, HexIdentifier> InternalDefinition() =>
        (_, iCoords) =>
            iCoords.kRef()
                .kIsType<HexIdentifier>()
                .kCatchNolla(
                () =>
                    Core.kCompose<HexIdentifierType>()
                        .kWithRovi(
                        Core.Hint<HexIdentifierType>(),
                        HexType.R,
                        iCoords.kRef()
                            .kGetRovi(HexType.R))
                        .kWithRovi(
                        Core.Hint<HexIdentifierType>(),
                        HexType.U,
                        iCoords.kRef()
                            .kGetRovi(HexType.U))
                        .kWithRovi(
                        Core.Hint<HexIdentifierType>(),
                        HexType.D,
                        iCoords.kRef()
                            .kGetRovi(HexType.D)));
}
