namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas.Game;

using u.Constructs.Move;
using u.Identifier;
using u.Data;
using u.Constructs.Resolved;
using u.Constructs;
using u;
using HexIdent = IRoveggi<u.Identifier.uHexIdentifier>;
using PlayerIdent = IRoveggi<u.Identifier.uPlayerIdentifier>;
using UnitIdent = IRoveggi<u.Identifier.uUnitIdentifier>;
using Core = Core.Syntax.Core;
using Infinite = Syntax.Infinite;

public static class GameProgressionCheck
{
    public static Korvessa<ProgramState, Bool> Construct(IKorssa<ProgramState> since) =>
        new(since)
        {
            Du = Axoi.Korvedu("GameProgressionCheck"),
            Definition =
                (_, iSince) =>
                    Infinite.AllPlayers
                        .kAnyMatch(
                        iPlayer =>
                            Core.kSubEnvironment<Bool>(
                            new()
                            {
                                Environment =
                                [
                                    Core.kSubEnvironment<IRoveggi<uPlayerData>>(
                                        new()
                                        {
                                            Environment = [iSince.kRef().kLoad()],
                                            Value = iPlayer.kRef().kRead(),
                                        })
                                        .kAsVariable(out var iOldData),
                                    iPlayer.kRef()
                                        .kRead()
                                        .kAsVariable(out var iCurrentData),
                                ],
                                Value =
                                    iCurrentData.kRef()
                                        .kGetRovi(uPlayerData.CONTROL)
                                        .kIsGreaterThan(iOldData.kRef().kGetRovi(uPlayerData.CONTROL)),
                            }))
                        .ksLazyOr(
                        Infinite.AllUnits
                            .kAnyMatch(
                            iUnit =>
                                Core.kSubEnvironment<Bool>(
                                new()
                                {
                                    Environment =
                                    [
                                        Core.kSubEnvironment<IRoveggi<uUnitData>>(
                                            new()
                                            {
                                                Environment = [iSince.kRef().kLoad()],
                                                Value = iUnit.kRef().kRead(),
                                            })
                                            .kAsVariable(out var iOldData),
                                        iUnit.kRef()
                                            .kRead()
                                            .kAsVariable(out var iCurrentData),
                                    ],
                                    Value =
                                        iOldData.kRef()
                                            .kGetRovi(uUnitData.HP)
                                            .kIsGreaterThan(iCurrentData.kRef().kGetRovi(uUnitData.HP)),
                                }))),
        };
}