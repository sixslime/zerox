namespace SixShaded.FourZeroOne.Axois.Infinite.Syntax;

using Rovetus.Identifier;
using Core = Core.Syntax.Core;
using U = Rovetus;
public static partial class Infinite
{
    public static IKorssa<IRoveggi<U.Anchors.uGameAnchor>> Game => Core.kCompose<U.Anchors.uGameAnchor>();
    public static IKorssa<IRoveggi<U.Anchors.uConfigAnchor>> Configuration => Core.kCompose<U.Anchors.uConfigAnchor>();
}

public static partial class KorssaSyntax
{

}