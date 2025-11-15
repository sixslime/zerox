namespace SixShaded.FourZeroOne.Axois.Infinite.Syntax;

using Rovetus.Identifier;
using Core = Core.Syntax.Core;
using u = Rovetus;
using u.Constructs.Ability;
using Korvessas.HexCoordinates;
using u.Constructs;
using HexOffset = IRoveggi<u.Constructs.uHexOffset>;
using HexIdentifier = IRoveggi<u.Identifier.uHexIdentifier>;
using HexCoords = IRoveggi<u.Constructs.uHexCoordinates>;
using HexPosition = IRoveggi<u.Identifier.uHexIdentifier>;

partial class KorssaSyntax
{
    public static Korssas.LineIntersections kLineIntersectionsTo(this IKorssa<HexCoords> from, IKorssa<HexCoords> to) => new(from, to);
    public static Add kAdd(this IKorssa<HexCoords> a, IKorssa<HexCoords> b) => new(a, b);
    public static Subtract kSubtract(this IKorssa<HexCoords> a, IKorssa<HexCoords> b) => new(a, b);
    public static AsOffset kAsOffset(this IKorssa<HexCoords> coords) => new(coords);
    public static AsAbsolute kAsAbsolute(this IKorssa<HexCoords> coords) => new(coords);
    public static AffixHitArea kAffixToUnit(this IKorssa<IMulti<HexOffset>> hitArea, IKorssa<IRoveggi<uUnitIdentifier>> unit) => new(hitArea, unit);
    public static RotateAround kRotateAround(this IKorssa<HexCoords> coords, IKorssa<HexCoords> anchor, IKorssa<Number> rotation) => new(coords, anchor, rotation);
    public static GetAdjacent kAdjacent(this IKorssa<HexCoords> hex) => new(hex);

    public static FourZeroOne.Core.Korssas.Fixed<HexOffset> kAsHex(this (int, int, int) coordinates) =>
        new(
        new Roveggi<uHexOffset>()
            .WithComponent(uHexCoordinates.R, coordinates.Item1)
            .WithComponent(uHexCoordinates.U, coordinates.Item2)
            .WithComponent(uHexCoordinates.D, coordinates.Item3));
}

partial class Infinite
{ }