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

public static class DoEliminateUnit
{
    public static Korvessa<UnitIdent, Rog> Construct(IKorssa<UnitIdent> unit) =>
        new(unit)
        {
            Du = Axoi.Korvedu("DoEliminateUnit"),
            Definition =
                (_, iUnit) =>
                    iUnit.kRef()
                        .kRedact(),
        };
}