namespace SixShaded.FourZeroOne.Axois.Infinite.Korssas;

using Handles;
using Helpers;
using MorseCode.ITask;
using HexObject = IRoveggi<u.Constructs.uRelativeCoordinate>;
using Hex = u.Constructs.uRelativeCoordinate;

// returns a multi of multis such that each element contains either 1 or 2 hex coordinates.
// 1 denotes a single normal intersection.
// 2 denotes an exact line split pair.
// this does not include the start/end hexes.
public record LineIntersections(IKorssa<HexObject> from, IKorssa<HexObject> to) : Korssa.Defined.Function<HexObject, HexObject, Multi<Multi<HexObject>>>(from, to)
{
    protected override ITask<IOption<Multi<Multi<HexObject>>>> Evaluate(IKorssaContext _, IOption<HexObject> in1, IOption<HexObject> in2)
    {
        if (in1.RemapAs(x => x.GetStruct()).Press().CheckNone(out var fromHex) || in1.RemapAs(x => x.GetStruct()).Press().CheckNone(out var toHex))
            return new None<Multi<Multi<HexObject>>>().ToCompletedITask();

        var hexTarget = toHex - fromHex;

        // check for perfect diagonal (i.e. contains perfect splits):
        int diagonalIndex =
            (0..3).ToIter(true)
            .FirstMatch(i => hexTarget[(i + 1) % 3] == hexTarget[(i + 2) % 3])
            .Or(-1);
        if (diagonalIndex != -1)
        {
            // will always be even:
            int targetValue = hexTarget[diagonalIndex];
            int step = Math.Sign(targetValue);
            var o =
                (hop: new HexPos(0, 0, 0),
                pair: (a: new HexPos(0, 0, 0), b: new HexPos(0, 0, 0)))
                .Sequence(
                x =>
                    (new HexPos(x.hop.R + (2 * step), x.hop.U - step, x.hop.D - step),
                    (new HexPos(x.hop.R + step, x.hop.U - step, x.hop.D), new HexPos(x.hop.R + step, x.hop.U, x.hop.D - step))))
                .Skip(1)
                .Map(
                x =>
                    Iter.Over(Iter.Over(x.pair.a, x.pair.b), x.hop.Yield()))
                .Flatten()
                .Take(Math.Abs(targetValue) - 1)
                .Map(x => x.Map(y => y.Shift(diagonalIndex) + fromHex));
            return FormatResult(o);
        }

        // check for perfect axis (i.e. straight line of hexes):
        int straightIndex =
            (0..3).ToIter(true)
            .FirstMatch(i => hexTarget[(i + 1) % 3] == -(hexTarget[(i + 2) % 3]))
            .Or(-1);

        if (straightIndex != -1)
        {
            int targetValue = hexTarget[(straightIndex + 1) % 3];
            int step = Math.Sign(targetValue);
            var o =
                new HexPos(0, 0, 0)
                    .Sequence(x => new HexPos(0, x.U + step, x.D - step))
                    .Skip(1)
                    .Take(Math.Abs(targetValue) - 1)
                    .Map(x => (x.Shift(straightIndex) + fromHex).Yield());
            return FormatResult(o);
        }
        {
            var o = 6;
        }
        // imperfect line, must do geometry:


        throw new NotImplementedException();
    }

    private ITask<IOption<Multi<Multi<HexObject>>>> FormatResult(IEnumerable<IEnumerable<HexPos>> resultValue) =>
        new Multi<Multi<HexObject>>
            {
                Values =
                    resultValue.Map(
                        val =>
                            new Multi<HexObject>
                            {
                                Values = val.Map(x => x.GetRoveggi().AsSome()).ToPSequence()
                            }.AsSome())
                        .ToPSequence()
            }.AsSome()
            .ToCompletedITask();
}