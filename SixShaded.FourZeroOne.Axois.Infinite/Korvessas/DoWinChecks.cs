namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas;

using u.Constructs.Move;
using u.Constructs.Resolved;
using u.Identifier;
using u.Data;
using Core = Core.Syntax.Core;
using u.Constructs;
using Infinite = Syntax.Infinite;

public static class DoWinChecks
{
    public static Korvessa<IRoveggi<uPlayerIdentifier>, IRoveggi<uWinChecks>> Construct(IKorssa<IRoveggi<uPlayerIdentifier>> player) =>
        new(player)
        {
            Du = Axoi.Korvedu("DoWinChecks"),
            Definition =
                (_, iPlayer) =>
                    Core.kSubEnvironment<IRoveggi<uWinChecks>>(new()
                    {
                        Environment =
                            [
                                iPlayer.kRef()
                                    .kRead()
                                    .kAsVariable(out var iData)
                            ],
                        Value =
                            Core.kCompose<uWinChecks>()
                                .kWithRovi(
                                uWinChecks.CONTROL,
                                iData.kRef()
                                    .kGetRovi(uPlayerData.CONTROL)
                                    .kIsGreaterThan(4.kFixed()))
                                .kWithRovi(
                                uWinChecks.LAST_ALIVE,
                                Infinite.AllUnits
                                    .kAllMatch(
                                    iUnit =>
                                        iUnit.kRef()
                                            .kRead()
                                            .kGetRovi(uUnitData.OWNER)
                                            .kEquals(iPlayer.kRef())))
                    })
        };
}