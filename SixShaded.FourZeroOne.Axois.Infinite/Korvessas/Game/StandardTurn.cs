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

                        // clear unit effects:
                        Infinite.AllUnits
                            .kMap(
                            iUnit =>
                                iUnit.kRef()
                                    .kSafeUpdate(
                                    iUnitData =>
                                        iUnitData.kRef()
                                            .kSafeUpdateRovi(
                                            uUnitData.EFFECTS,
                                            iEffects =>
                                                iEffects.kRef()
                                                    .kWhere(
                                                    iEffect =>
                                                        iEffect.kRef()
                                                            .kGetRovi(uUnitEffect.INFLICTED_BY)
                                                            .kEquals(iPlayer.kRef())
                                                            .kNot())))),
                    })
        };
}