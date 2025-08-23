namespace SixShaded.FourZeroOne.Axois.Infinite.Syntax;

using Rovetus.Identifier;
using Core = Core.Syntax.Core;
using u = Rovetus;
using u.Constructs.Ability;
using u.Constructs;
using HexOffset = IRoveggi<u.Constructs.uHexOffset>;
using HexIdentifier = IRoveggi<u.Identifier.uHexIdentifier>;
using HexCoords = IRoveggi<u.Constructs.uHexCoordinates>;
using HexPosition = IRoveggi<u.Identifier.uHexIdentifier>;

partial class KorssaSyntax
{
    public static Korssas.LineIntersections kLineIntersectionsTo(this IKorssa<HexCoords> from, IKorssa<HexCoords> to) => new(from, to);
    public static Korvessa<HexCoords, HexCoords, HexOffset> kAdd(this IKorssa<HexCoords> a, IKorssa<HexCoords> b) => Korvessas.HexCoordinates.Add.Construct(a, b);
    public static Korvessa<HexCoords, HexCoords, HexOffset> kSubtract(this IKorssa<HexCoords> a, IKorssa<HexCoords> b) => Korvessas.HexCoordinates.Subtract.Construct(a, b);
    public static Korvessa<HexCoords, HexOffset> kAsOffset(this IKorssa<HexCoords> coords) => Korvessas.HexCoordinates.AsOffset.Construct(coords);
    public static Korvessa<HexCoords, HexIdentifier> kAsAbsolute(this IKorssa<HexCoords> coords) => Korvessas.HexCoordinates.AsAbsolute.Construct(coords);
    public static Korvessa<IMulti<HexOffset>, IRoveggi<uUnitIdentifier>, Multi<HexIdentifier>> kAffixToUnit(this IKorssa<IMulti<HexOffset>> hitArea, IKorssa<IRoveggi<uUnitIdentifier>> unit) => Korvessas.HexCoordinates.AffixHitArea.Construct(hitArea, unit);
    public static Korvessa<HexCoords, HexCoords, Number, HexOffset> kRotateAround(this IKorssa<HexCoords> coords, IKorssa<HexCoords> anchor, IKorssa<Number> rotation) => Korvessas.HexCoordinates.RotateAround.Construct(coords, anchor, rotation);
    public static Korvessa<HexCoords, Multi<HexOffset>> kAdjacent(this IKorssa<HexCoords> hex) => Korvessas.HexCoordinates.GetAdjacent.Construct(hex);

    public static FourZeroOne.Core.Korssas.Fixed<HexOffset> kAsHex(this (int, int, int) coordinates) =>
        new(
        new Roveggi<uHexOffset>()
            .WithComponent(uHexCoordinates.R, coordinates.Item1)
            .WithComponent(uHexCoordinates.U, coordinates.Item2)
            .WithComponent(uHexCoordinates.D, coordinates.Item3));
}

partial class Infinite
{ }