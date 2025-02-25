namespace SixShaded.FourZeroOne.Core.Korssas.Multi;

using Roggis;

public sealed record GetIndex<R> : Korssa.Defined.Function<IMulti<R>, Number, R> where R : class, Rog
{
    public GetIndex(IKorssa<IMulti<R>> from, IKorssa<Number> index) : base(from, index) { }

    protected override ITask<IOption<R>> Evaluate(IKorssaContext _, IOption<IMulti<R>> in1, IOption<Number> in2)
    {
        var o = in1.Check(out var from) && in2.Check(out var index)
            ? from.At(index.Value - 1)
            : new None<R>();
        return o.ToCompletedITask();
    }

    protected override IOption<string> CustomToString() => $"{Arg1}[{Arg2}]".AsSome();
}