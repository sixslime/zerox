namespace SixShaded.FourZeroOne.Axois.Infinite.Syntax;

using Rovetus.Identifier;
using Core = Core.Syntax.Core;
using u = Rovetus;
using u.Constructs.Ability;
using u.Constructs;
using HexObject = IRoveggi<u.Constructs.uRelativeCoordinate>;

partial class KorssaSyntax
{
    public static Korssas.LineIntersections kLineIntersectionsTo(this IKorssa<HexObject> from, IKorssa<HexObject> to) => new(from, to);
}