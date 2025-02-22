#nullable enable
namespace SixShaded.FourZeroOne.Core.Tokens
{
    
    public record ToBoxedArgs<R1> : Token.Defined.Function<R1, MetaArgs<R1>>
        where R1 : class, Res
    {
        public ToBoxedArgs(IToken<R1> in1) : base(in1) { }
        protected override ITask<IOption<MetaArgs<R1>>> Evaluate(ITokenContext _, IOption<R1> in1)
        {
            return new MetaArgs<R1>() { Arg1 = in1 }.AsSome().ToCompletedITask();
        }
        protected override IOption<string> CustomToString() => $"<${Arg1}>".AsSome();
    }
    public record ToBoxedArgs<R1, R2> : Token.Defined.Function<R1, R2, MetaArgs<R1, R2>>
        where R1 : class, Res
        where R2 : class, Res
    {
        public ToBoxedArgs(IToken<R1> in1, IToken<R2> in2) : base(in1, in2) { }
        protected override ITask<IOption<MetaArgs<R1, R2>>> Evaluate(ITokenContext _, IOption<R1> in1, IOption<R2> in2)
        {
            return new MetaArgs<R1, R2>() { Arg1 = in1, Arg2 = in2 }.AsSome().ToCompletedITask();
        }
        protected override IOption<string> CustomToString() => $"<${Arg1} ${Arg2}>".AsSome();
    }
    public record ToBoxedArgs<R1, R2, R3> : Token.Defined.Function<R1, R2, R3, MetaArgs<R1, R2, R3>>
        where R1 : class, Res
        where R2 : class, Res
        where R3 : class, Res
    {
        public ToBoxedArgs(IToken<R1> in1, IToken<R2> in2, IToken<R3> in3) : base(in1, in2, in3) { }
        protected override ITask<IOption<MetaArgs<R1, R2, R3>>> Evaluate(ITokenContext _, IOption<R1> in1, IOption<R2> in2, IOption<R3> in3)
        {
            return new MetaArgs<R1, R2, R3>() { Arg1 = in1, Arg2 = in2, Arg3 = in3 }.AsSome().ToCompletedITask();
        }
        protected override IOption<string> CustomToString() => $"<${Arg1} ${Arg2} ${Arg3}>".AsSome();
    }
}