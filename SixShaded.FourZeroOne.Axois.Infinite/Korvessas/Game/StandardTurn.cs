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
public static class GetProgressionPredicate
{
    public static Korvessa<MetaFunction<Bool>> Construct() =>
        new()
        {
            Du = Axoi.Korvedu("GetProgressionPredicate"),
            Definition =
                (_) =>
                    Core.kSubEnvironment<MetaFunction<Bool>>(new()
                    {
                        Environment =
                            [
                                Core.kAllRovedanggiValues<uPlayerIdentifier, IRoveggi<uPlayerData>>()
                                    .kAsVariable(out var iPlayerDatas)
                            ],
                        Value =
                            iUnitDatas.kR
                    })
        };
}