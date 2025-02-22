namespace SixShaded.FourZeroOne.Core.Tokens.Multi;

using Resolutions;
public sealed record Count : Token.Defined.Function<IMulti<Res>, Number>
{
    public Count(IToken<IMulti<Res>> of) : base(of) { }
    protected override ITask<IOption<Number>> Evaluate(ITokenContext _, IOption<IMulti<Res>> in1) => new Number { Value = in1.RemapAs(x => x.Count).Or(0) }.AsSome().ToCompletedITask();
    protected override IOption<string> CustomToString() => $"{Arg1}.len".AsSome();
}