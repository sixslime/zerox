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
public static class StandardTurn
{
    public static Korvessa<PlayerIdent, Rog> Construct(IKorssa<PlayerIdent> player) =>
        new(player)
        {
            Du = Axoi.Korvedu("StandardTurn"),
            Definition =
                (_, iPlayer) =>
                    Core.kMulti<Rog>(
                    new()
                    {
                        iPlayer.kRef()
                            .kDoUnitEffectCycle(),
                        iPlayer.kRef()
                            .kSafeUpdate(iPlayerData => 
                                iPlayerData.kRef()
                                    .kWithRefreshedEnergy()
                                    .kWithRestockedHand()),
                        iPlayer.kRef()
                            .kAllowPlay()
                            .kMap(iAction => iAction.kRef().kGetRovi(uResolved.INSTRUCTIONS))
                    })
        };
}