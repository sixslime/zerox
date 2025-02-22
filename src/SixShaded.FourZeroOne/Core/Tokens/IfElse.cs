#nullable enable
using FourZeroOne;

namespace SixShaded.FourZeroOne.Core.Tokens
{
    public record IfElse<R> : Function<ro.Bool, r.Boxed.MetaFunction<R>, r.Boxed.MetaFunction<R>, r.Boxed.MetaFunction<R>> where R : Res
    {
        public IfElse(IToken<ro.Bool> condition, IToken<r.Boxed.MetaFunction<R>> positive, IToken<r.Boxed.MetaFunction<R>> negative) : base(condition, positive, negative) { }
        protected override ITask<IOption<r.Boxed.MetaFunction<R>>> Evaluate(ITokenContext runtime, IOption<ro.Bool> in1, IOption<r.Boxed.MetaFunction<R>> in2, IOption<r.Boxed.MetaFunction<R>> in3)
        {
            return in1.RemapAs(x => x.IsTrue ? in2 : in3).Press().ToCompletedITask();
        }
        protected override IOption<string> CustomToString() => $"if {Arg1} then {Arg2} else {Arg3}".AsSome();
    }
}