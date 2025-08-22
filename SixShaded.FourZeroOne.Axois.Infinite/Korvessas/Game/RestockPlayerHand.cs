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

public static class RestockPlayerHand
{
    public static Korvessa<IRoveggi<uPlayerData>, IRoveggi<uPlayerData>> Construct(IKorssa<IRoveggi<uPlayerData>> playerData) =>
        new(playerData)
        {
            Du = Axoi.Korvedu("RestockPlayerHand"),
            Definition =
                (_, iPlayerData) =>
                    Core.kSubEnvironment<IRoveggi<uPlayerData>>(
                    new()
                    {
                        Environment =
                        [
                            iPlayerData.kRef()
                                .kGetRovi(uPlayerData.HAND_SIZE)
                                .kSubtract(
                                iPlayerData.kRef()
                                    .kGetRovi(uPlayerData.HAND)
                                    .kCount())
                                .kAsVariable(out var iDrawAmount),
                        ],
                        Value =
                            iDrawAmount.kRef()
                                .kIsGreaterThan(0.kFixed())
                                .kIfTrue<IRoveggi<uPlayerData>>(
                                new()
                                {
                                    Then =
                                        Core.kSubEnvironment<IRoveggi<uPlayerData>>(
                                        new()
                                        {
                                            Environment =
                                            [
                                                iPlayerData.kRef()
                                                    .kGetRovi(uPlayerData.STACK)
                                                    .kGetSlice(1.kFixed().kRangeTo(iDrawAmount.kRef()))
                                                    .kAsVariable(out var iDrawnAbilities)
                                            ],
                                            Value =
                                                iPlayerData.kRef()
                                                    .kSafeUpdateRovi(
                                                    uPlayerData.STACK,
                                                    iStack =>
                                                        iStack.kRef()
                                                            .kGetSlice(
                                                            iDrawAmount.kRef()
                                                                .kAdd(1.kFixed())
                                                                .kRangeTo(iStack.kRef().kCount())))
                                                    .kSafeUpdateRovi(
                                                    uPlayerData.HAND,
                                                    iHand =>
                                                        iHand.kRef().kConcat(iDrawnAbilities.kRef()))
                                        }),
                                    Else = iPlayerData.kRef()
                                })
                    })
        };
}