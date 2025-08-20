namespace SixShaded.FourZeroOne.Axois.Infinite.Korssas;

using Handles;
using Helpers;
using MorseCode.ITask;
using HexCoords = IRoveggi<u.Constructs.uHexCoordinates>;
using HexOffset = IRoveggi<u.Constructs.uHexOffset>;

// returns a multi of multis such that each element contains either 1 or 2 hex coordinates.
// 1 denotes a single normal intersection.
// 2 denotes an exact line split pair.
// this does not include the start/end hexes.
public record LineIntersections(IKorssa<HexCoords> from, IKorssa<HexCoords> to) : Korssa.Defined.Function<HexCoords, HexCoords, Multi<Multi<HexOffset>>>(from, to)
{
    protected override ITask<IOption<Multi<Multi<HexOffset>>>> Evaluate(IKorssaContext _, IOption<HexCoords> in1, IOption<HexCoords> in2)
    {
        if (in1.RemapAs(x => x.GetStruct()).Press().CheckNone(out var fromHex) || in1.RemapAs(x => x.GetStruct()).Press().CheckNone(out var toHex))
            return new None<Multi<Multi<HexOffset>>>().ToCompletedITask();
        var hexTarget = toHex - fromHex;

        // check for perfect diagonal (i.e. contains perfect splits):
        int diagonalIndex =
            (..3).ToIter(true)
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
                    (new(x.hop.R + (2 * step), x.hop.U - step, x.hop.D - step),
                    (new(x.hop.R + step, x.hop.U - step, x.hop.D), new(x.hop.R + step, x.hop.U, x.hop.D - step))))
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
            (..3).ToIter(true)
            .FirstMatch(i => hexTarget[(i + 1) % 3] == -hexTarget[(i + 2) % 3])
            .Or(-1);
        if (straightIndex != -1)
        {
            int targetValue = hexTarget[(straightIndex + 1) % 3];
            int step = Math.Sign(targetValue);
            var o =
                new HexPos(0, 0, 0)
                    .Sequence(x => new(0, x.U + step, x.D - step))
                    .Skip(1)
                    .Take(Math.Abs(targetValue) - 1)
                    .Map(x => (x.Shift(straightIndex) + fromHex).Yield());
            return FormatResult(o);
        }

        // not perfect straight or diagonal, normal algorithm:
        return FormatResult(
        GeneralAlgorithm(fromHex, toHex)
            .Map(x => x.Yield()));
    }

    // me when i fucking chatGPT
    // sad that i didnt think of this algorithm; i should kill myself
    private List<HexPos> GeneralAlgorithm(HexPos start, HexPos end)
    {
        
        var results = new List<HexPos>();
        int n = start.DistanceTo(end);
        for (int i = 1; i < n; i++) // skip start (0) and end (n)
        {
            double t = (double)i / n;
            results.Add(__Round(__Lerp(start, end, t)));
        }
        return results;

        (double R, double U, double D) __Lerp(HexPos a, HexPos b, double t)
        {
            return (
            a.R + ((b.R - a.R) * t),
            a.U + ((b.U - a.U) * t),
            a.D + ((b.D - a.D) * t)
            );
        }
        HexPos __Round((double R, double U, double D) h)
        {
            int rr = (int)Math.Round(h.R);
            int ru = (int)Math.Round(h.U);
            int rd = (int)Math.Round(h.D);
            double rDiff = Math.Abs(rr - h.R);
            double uDiff = Math.Abs(ru - h.U);
            double dDiff = Math.Abs(rd - h.D);
            if (rDiff > uDiff && rDiff > dDiff) rr = -ru - rd;
            else if (uDiff > dDiff) ru = -rr - rd;
            else rd = -rr - ru;
            return new(rr, ru, rd);
        }
    }

    private ITask<IOption<Multi<Multi<HexOffset>>>> FormatResult(IEnumerable<IEnumerable<HexPos>> resultValue) =>
        new Multi<Multi<HexOffset>>
            {
                Values =
                    resultValue.Map(
                        val =>
                            new Multi<HexOffset>
                            {
                                Values = val.Map(x => x.GetRoveggi().AsSome()).ToPSequence(),
                            }.AsSome())
                        .ToPSequence(),
            }.AsSome()
            .ToCompletedITask();
}