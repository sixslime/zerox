#nullable enable
namespace SixShaded.FourZeroOne.Core.Tokens
{
    
    using Resolutions;
    public record IfElse<R> : Token.Defined.Function<Bool, MetaFunction<R>, MetaFunction<R>, MetaFunction<R>> where R : class, Res
    {
        public IfElse(IToken<Bool> condition, IToken<MetaFunction<R>> positive, IToken<MetaFunction<R>> negative) : base(condition, positive, negative) { }
        protected override ITask<IOption<MetaFunction<R>>> Evaluate(ITokenContext runtime, IOption<Bool> in1, IOption<MetaFunction<R>> in2, IOption<MetaFunction<R>> in3)
        {
            return in1.RemapAs(x => x.IsTrue ? in2 : in3).Press().ToCompletedITask();
        }
        protected override IOption<string> CustomToString() => $"if {Arg1} then {Arg2} else {Arg3}".AsSome();
    }
}