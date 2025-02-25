namespace SixShaded.FourZeroOne.Core.Tokens.Multi;

using Resolutions;

public sealed record Contains<R> : Token.Defined.Function<IMulti<R>, R, Bool> where R : class, Res
{
    public Contains(IToken<IMulti<R>> multi, IToken<R> element) : base(multi, element) { }
    protected override ITask<IOption<Bool>> Evaluate(ITokenContext runtime, IOption<IMulti<R>> in1, IOption<R> in2) => in2.RemapAs(item => new Bool { IsTrue = in1.RemapAs(arr => arr.Elements.Contains(item)).Or(false) }).ToCompletedITask();
}