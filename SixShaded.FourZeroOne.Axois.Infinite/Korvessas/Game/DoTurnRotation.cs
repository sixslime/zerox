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
using u.Constructs.GameResults;
public static class GameLoop
{
    public static Korvessa<Rog> Construct() =>
        new()
        {
            Du = Axoi.Korvedu("TurnRotation"),
            Definition =
                (iNextLoop) =>
                    Core.kSubEnvironment<IRoveggi<uGameResult>>(
                    new()
                    {
                        Environment =
                        [
                            Infinite.kDoCycleTurnOrder(),
                            Infinite.Game
                                .kRead()
                                .kGetRovi(uGame.TURN_ORDER)
                                .kGetIndex(Infinite.Game.kRead().kGetRovi(uGame.TURN_INDEX))
                                .kAsVariable(out var iCurrentPlayer),
                            Infinite.kDoStandardTurn(iCurrentPlayer.kRef()),
                            Infinite.kCheckGameResult()
                                .kAsVariable(out var iResult)
                        ],
                        Value =
                            iResult.kRef()
                                .kExists()
                                .kIfTrue<IRoveggi<uGameResult>>(
                                new()
                                {
                                    Then = iResult.kRef(),
                                    Else = iNextLoop.kRef().kExecute()
                                })
                    })
        };
}