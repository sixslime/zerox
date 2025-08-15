namespace SixShaded.FourZeroOne.Axois.Infinite.Syntax;

using Rovetus.Identifier;
using Core = Core.Syntax.Core;
using u = Rovetus;
using u.Constructs.Ability;
using u.Constructs;
using HexOffset = IRoveggi<u.Constructs.uRelativeCoordinate>;
using HexOffsetType = u.Constructs.uRelativeCoordinate;
using HexPosition = IRoveggi<u.Identifier.uHexCoordinate>;

partial class KorssaSyntax
{
    public static Korssas.LineIntersections kLineIntersectionsTo(this IKorssa<HexOffset> from, IKorssa<HexOffset> to) => new(from, to);

    public static FourZeroOne.Core.Korssas.Fixed<HexOffset> kAsHex(this (int, int, int) coordinates) =>
        new(
        new Roveggi<HexOffsetType>()
            .WithComponent(HexOffsetType.R, coordinates.Item1)
            .WithComponent(HexOffsetType.U, coordinates.Item2)
            .WithComponent(HexOffsetType.D, coordinates.Item3));
}

partial class Infinite
{
}