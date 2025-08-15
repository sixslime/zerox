namespace SixShaded.FourZeroOne.Axois.Infinite.Syntax;

using u.Anchors;
using u.Identifier;
using u.Data;
using Core = Core.Syntax.Core;
public static partial class Infinite
{
    public static IKorssa<IRoveggi<uGameAnchor>> Game => Core.kCompose<uGameAnchor>();
    public static IKorssa<IRoveggi<uConfigAnchor>> Configuration => Core.kCompose<uConfigAnchor>();
    public static IKorssa<IRoveggi<uPlayerIdentifier>> CurrentPlayer => Game.kGet().kGetRovi(u.uGame.CURRENT_PLAYER);
    public static IKorssa<IMulti<IRoveggi<uUnitIdentifier>>> AllUnits => Core.kAllRovedanggiKeys<uUnitIdentifier, IRoveggi<uUnitData>>();
}

public static partial class KorssaSyntax
{

}