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
using u.Constructs.EffectTypes;
public static class UnitEffectCycle
{
    public static Korvessa<PlayerIdent, Rog> Construct(IKorssa<PlayerIdent> player) =>
        new(player)
        {
            Du = Axoi.Korvedu("UnitEffectCycle"),
            Definition =
                (_, iPlayer) =>
                    Infinite.AllUnits
                        .kMap(
                        iUnit =>
                            iUnit.kRef()
                                .kSafeUpdate(
                                iUnitData =>
                                    Core.kSubEnvironment<IRoveggi<uUnitData>>(new()
                                    {
                                        Environment =
                                            [
                                                iUnitData.kRef()
                                                    .kGetRovi(uUnitData.EFFECTS)
                                                    .kWhere(
                                                    iEffect =>
                                                        iEffect.kRef()
                                                            .kGetRovi(uUnitEffect.INFLICTED_BY)
                                                            .kEquals(iPlayer.kRef()))
                                                    .kAsVariable(out var iInflictedEffects),
                                            ],
                                        Value =
                                            iUnitData.kRef()
                                                .kSafeUpdateRovi(
                                                uUnitData.HP, 
                                                iHp =>
                                                    iHp.kRef()
                                                        .kSubtract(iInflictedEffects.kRef()
                                                            .kWhere(
                                                            iEffect =>
                                                                iEffect.kRef()
                                                                    .kGetRovi(uUnitEffect.TYPE)
                                                                    .kIsType<uDamageEffect>()
                                                                    .kExists())
                                                            .kCount()))
                                                .kSafeUpdateRovi(
                                                uUnitData.EFFECTS,
                                                iEffects =>
                                                    iEffects.kRef()
                                                        .kWhere(
                                                        iEffect =>
                                                            iEffects.kRef()
                                                                .kContains(iEffect.kRef())
                                                                .kNot()))
                                    })))
        };
}