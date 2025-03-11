namespace SixShaded.FourZeroOne.Core.Korssas.Multi;

using Roggis;

public sealed record Count : Korssa.Defined.Function<IMulti<Rog>, Number>
{
    public Count(IKorssa<IMulti<Rog>> of) : base(of)
    { }

    protected override ITask<IOption<Number>> Evaluate(IKorssaContext _, IOption<IMulti<Rog>> in1) =>
        new Number
            {
                Value = in1.RemapAs(x => x.Count).Or(0),
            }.AsSome()
            .ToCompletedITask();

    protected override IOption<string> CustomToString() => $"{Arg1}.len".AsSome();
}