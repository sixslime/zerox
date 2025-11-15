namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas.HexCoordinates;

using Core = Core.Syntax.Core;
using HexCoords = IRoveggi<u.Constructs.uHexCoordinates>;
using HexOffset = IRoveggi<u.Constructs.uHexOffset>;
using HexIdentifier = IRoveggi<u.Identifier.uHexIdentifier>;
using HexType = u.Constructs.uHexCoordinates;

public record AffixHitArea(IKorssa<IMulti<HexOffset>> hitArea, IKorssa<IRoveggi<u.Identifier.uUnitIdentifier>> unit) : Korvessa<IMulti<HexOffset>, IRoveggi<u.Identifier.uUnitIdentifier>, Multi<HexIdentifier>>(hitArea, unit)
{
    protected override RecursiveMetaDefinition<IMulti<HexOffset>, IRoveggi<u.Identifier.uUnitIdentifier>, Multi<HexIdentifier>> InternalDefinition() =>
        (_, iHitArea, iUnit) =>
            Core.kSubEnvironment<Multi<HexIdentifier>>(
            new()
            {
                Environment =
                [
                    iUnit.kRef()
                        .kRead()
                        .kGetRovi(u.Data.uUnitData.OWNER)
                        .kRead()
                        .kGetRovi(u.Data.uPlayerData.PERSPECTIVE_ROTATION)
                        .kAsVariable(out var iPerspective),
                    iUnit.kRef()
                        .kRead()
                        .kGetRovi(u.Data.uUnitData.POSITION)
                        .kAsVariable(out var iPosition),
                ],
                Value =
                    iHitArea.kRef()
                        .kMap(
                        iHex =>
                            iHex.kRef()
                                .kRotateAround(
                                (0, 0, 0).kAsHex(),
                                iPerspective.kRef())
                                .kAdd(iPosition.kRef())
                                .kAsAbsolute()),
            });
}
