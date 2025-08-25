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

public record RefreshPlayerEnergy(IKorssa<IRoveggi<uPlayerData>> playerData) : Korvessa<IRoveggi<uPlayerData>, IRoveggi<uPlayerData>>(playerData)
{
    protected override RecursiveMetaDefinition<IRoveggi<uPlayerData>, IRoveggi<uPlayerData>> InternalDefinition() =>
        (_, iPlayerData) =>
            iPlayerData.kRef().kWithRovi(uPlayerData.ENERGY, 2.kFixed());
}