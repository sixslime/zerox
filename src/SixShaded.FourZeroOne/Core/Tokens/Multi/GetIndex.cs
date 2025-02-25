namespace SixShaded.FourZeroOne.Core.Tokens.Multi;

using Resolutions;

public sealed record GetIndex<R> : Token.Defined.Function<IMulti<R>, Number, R> where R : class, Res
{
    public GetIndex(IToken<IMulti<R>> from, IToken<Number> index) : base(from, index) { }

    protected override ITask<IOption<R>> Evaluate(ITokenContext _, IOption<IMulti<R>> in1, IOption<Number> in2)
    {
        var o = in1.Check(out var from) && in2.Check(out var index)
            ? from.At(index.Value - 1)
            : new None<R>();
        return o.ToCompletedITask();
    }

    protected override IOption<string> CustomToString() => $"{Arg1}[{Arg2}]".AsSome();
}