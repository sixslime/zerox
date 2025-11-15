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

public record DoEliminateUnit(IKorssa<UnitIdent> unit) : Korvessa<UnitIdent, Rog>(unit)
{
    protected override RecursiveMetaDefinition<UnitIdent, Rog> InternalDefinition() =>
        (_, iUnit) =>
            iUnit.kRef()
                .kRedact();
}