namespace SixShaded.FourZeroOne.Core.Roggis;

public sealed record NumRange : Roggi.Defined.NoOp, IMulti<Number>
{
    public required Number Start { get; init; }
    public required Number End { get; init; }

    public static implicit operator NumRange(Range range) =>
        new()
        {
            Start = range.Start.Value,
            End = range.End.Value,
        };

    public override string ToString() => $"{Start}..{End}";
    public int Count => Start.Value <= End.Value ? End.Value - Start.Value + 1 : 0;

    public IEnumerable<Number> Numbers =>
        Start.Value <= End.Value
            ? Start.Sequence(x => x.Value + 1).TakeWhile(x => x.Value <= End.Value)
            : [];

    IEnumerable<IOption<Number>> IHasElements<IOption<Number>>.Elements => Numbers.Map(x => x.AsSome());

    public IOption<IOption<Number>> At(int index) =>
        (index <= End.Value - Start.Value)
        .ToOption(
        new Number
        {
            Value = Start.Value + index,
        }.AsSome());
}