namespace SixShaded.FourZeroOne.Axois.Infinite.Syntax;

using Core = Core.Syntax.Core;
public static partial class Infinite
{
    public static IKorssa<IRoveggi<u.Anchors.uGameAnchor>> Game => Core.kCompose<u.Anchors.uGameAnchor>();
    public static IKorssa<IRoveggi<u.Anchors.uConfigAnchor>> Configuration => Core.kCompose<u.Anchors.uConfigAnchor>();
    public static IKorssa<IRoveggi<u.Identifier.uPlayerIdentifier>> CurrentPlayer => Game.kGet().kGetRovi(u.uGame.CURRENT_PLAYER);
}

public static partial class KorssaSyntax
{

}