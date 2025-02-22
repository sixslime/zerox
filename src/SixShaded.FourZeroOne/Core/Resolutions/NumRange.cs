#nullable enable
namespace SixShaded.FourZeroOne.Core.Resolutions
{
    public sealed record NumRange : Resolution.Defined.NoOp, IMulti<Number>
    {
        public required Number Start { get; init; }
        public required Number End { get; init; }
        public int Count => Start.Value <= End.Value ? End.Value - Start.Value + 1 : 0;

        public static implicit operator NumRange(Range range)
            => new() { Start = range.Start.Value, End = range.End.Value };
        public IEnumerable<Number> Elements =>
            Start.Value <= End.Value
                ? Start.Sequence(x => x.Value + 1).TakeWhile(x => x.Value <= End.Value)
                : [];

        public IOption<Number> At(int index)
        {
            return index <= End.Value - Start.Value
                ? new Number() { Value = Start.Value + index }.AsSome()
                : new None<Number>();
        }

        public override string ToString() => $"{Start}..{End}";
    }
}