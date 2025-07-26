namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas;

using Core = Core.Syntax.Core;
using u.Config;
using u;
using u.Identifier;
using u.Data;
using Infinite = Syntax.Infinite;

public static class SetupGame
{
    public static Korvessa<IRoveggi<uGameConfiguration>, Rog> Construct(IKorssa<IRoveggi<uGameConfiguration>> config) =>
        new(config)
        {
            Du = Axoi.Korvedu("SetupGame"),
            Definition =
                (_, iConfig) =>
                    Core.kSubEnvironment<Rog>(
                    new()
                    {
                        Environment =
                        [
                            // init:
                            Core.kMulti<Rog>(
                                Infinite.Game.kWrite(Core.kCompose<uGame>()),
                                Core.kAllRovedanggiKeys<uPlayerIdentifier, IRoveggi<uPlayerData>>()
                                    .kMap(
                                    iIdentifier =>
                                        iIdentifier.kRef().kRedact()),
                                Core.kAllRovedanggiKeys<uHexCoordinate, IRoveggi<uHexData>>()
                                    .kMap(
                                    iIdentifier =>
                                        iIdentifier.kRef().kRedact()),
                                Core.kAllRovedanggiKeys<uUnitIdentifier, IRoveggi<uUnitData>>()
                                    .kMap(
                                    iIdentifier =>
                                        iIdentifier.kRef().kRedact()))
                                .kAsVariable(out var iMakeInit),
                            // map:
                            iConfig.kRef()
                                .kGetVarovaKeys(uGameConfiguration.MAP)
                                .kMap(
                                iCoordinate =>
                                    iCoordinate.kRef()
                                        .kWrite(
                                        Core.kCompose<uHexData>()
                                            .kWithRovi(uHexData.TYPE, iConfig.kRef().kGetVarovi(uGameConfiguration.MAP, iCoordinate.kRef()))))
                                .kAsVariable(out var iMakeMap),
                            // players:
                            iConfig.kRef()
                                .kGetRovi(uGameConfiguration.PLAYERS)
                                .kMapWithIndex(
                                (iDeclaredPlayer, iIndex) =>
                                    Core.kSubEnvironment<Rog>(
                                    new()
                                    {
                                        Environment =
                                        [
                                            Core.kCompose<uPlayerIdentifier>()
                                                .kWithRovi(uPlayerIdentifier.NUMBER, iIndex.kRef())
                                                .kAsVariable(out var iIdentifier),
                                        ],
                                        Value =
                                            Core.kMulti<Rog>()
                                    }))
                        ],
                        Value =
                            Core.kMulti<Rog>(
                            iMakeInit.kRef(),
                            iMakeMap.kRef())
                    })
        }
}