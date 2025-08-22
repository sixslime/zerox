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

/// <summary>
/// Intended to be executed before resolving each turn.
/// </summary>
public static class DoTurnCycle
{
    public static Korvessa<Rog> Construct() =>
        new()
        {
            Du = Axoi.Korvedu("DoTurnCycle"),
            Definition =
                (_) =>
                    Core.kMulti<Rog>(
                    new()
                    {
                        Infinite.Game
                            .kUpdate(
                            iGame =>
                                iGame.kRef()
                                    .kGetRovi(uGame.TURN_INDEX)
                                    .kAdd(1.kFixed())
                                    .kIsGreaterThan(
                                    iGame.kRef()
                                        .kGetRovi(uGame.TURN_ORDER)
                                        .kCount())
                                    .kIfTrue<IRoveggi<uGame>>(
                                    new()
                                    {
                                        Then =
                                            iGame.kRef()
                                                .kUpdateRovi(uGame.ROTATION_COUNT, iCount => iCount.kRef().kAdd(1.kFixed()))
                                                .kWithRovi(uGame.TURN_INDEX, 1.kFixed())
                                                .kUpdateRovi(
                                                uGame.DEAD_ROTATIONS,
                                                iCount =>
                                                    iGame.kRef()
                                                        .kGetRovi(uGame.LAST_ROTATION_STATE)
                                                        .kHasGameProgressedSince()
                                                        .kIfTrue<Number>(
                                                        new()
                                                        {
                                                            Then = 0.kFixed(),
                                                            Else = iCount.kRef().kAdd(1.kFixed())
                                                        }))
                                                .kWithRovi(uGame.LAST_ROTATION_STATE, Core.kGetProgramState()),

                                        Else =
                                            iGame.kRef()
                                                .kUpdateRovi(uGame.TURN_INDEX, iIndex => iIndex.kRef().kAdd(1.kFixed()))
                                    }))
                    })
        };
}