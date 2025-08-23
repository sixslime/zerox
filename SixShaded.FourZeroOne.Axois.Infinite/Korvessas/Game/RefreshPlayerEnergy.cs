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

public static class RefreshPlayerEnergy
{
    public static Korvessa<IRoveggi<uPlayerData>, IRoveggi<uPlayerData>> Construct(IKorssa<IRoveggi<uPlayerData>> playerData) =>
        new(playerData)
        {
            Du = Axoi.Korvedu("RefreshPlayerEnergy"),
            Definition =
                (_, iPlayerData) =>
                    iPlayerData.kRef().kWithRovi(uPlayerData.ENERGY, 2.kFixed()),
        };
}