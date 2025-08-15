namespace SixShaded.FourZeroOne.Axois.Infinite.Syntax;

using Rovetus.Identifier;
using Core = Core.Syntax.Core;
using u = Rovetus;
using u.Constructs.Ability;
using u.Constructs;
using HexOffset = IRoveggi<u.Constructs.uHexOffset>;
using HexPosition = IRoveggi<u.Identifier.uHexIdentifier>;

partial class KorssaSyntax
{
    public static Korssas.LineIntersections kLineIntersectionsTo(this IKorssa<HexOffset> from, IKorssa<HexOffset> to) => new(from, to);

    public static FourZeroOne.Core.Korssas.Fixed<HexOffset> kAsHex(this (int, int, int) coordinates) =>
        new(
        new Roveggi<uHexOffset>()
            .WithComponent(uHexOffset.R, coordinates.Item1)
            .WithComponent(uHexOffset.U, coordinates.Item2)
            .WithComponent(uHexOffset.D, coordinates.Item3));
}

partial class Infinite
{
}