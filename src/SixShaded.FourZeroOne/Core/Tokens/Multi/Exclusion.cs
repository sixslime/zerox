#nullable enable
namespace SixShaded.FourZeroOne.Core.Tokens.Multi
{
    public sealed record Exclusion<R> : Token.Defined.Function<IMulti<R>, IMulti<R>, Resolutions.Multi<R>> where R : class, Res
    {
        public Exclusion(IToken<IMulti<R>> from, IToken<IMulti<R>> exclude) : base(from, exclude) { }
        protected override ITask<IOption<Resolutions.Multi<R>>> Evaluate(ITokenContext _, IOption<IMulti<R>> in1, IOption<IMulti<R>> in2)
        {
            return in1.RemapAs(from => new Resolutions.Multi<R>() { Values = in2.RemapAs(sub => from.Elements.Except(sub.Elements)).Or([]).ToPSequence() })
                .ToCompletedITask();
        }
        protected override IOption<string> CustomToString() => $"{Arg1} - {Arg2}".AsSome();
    }
}