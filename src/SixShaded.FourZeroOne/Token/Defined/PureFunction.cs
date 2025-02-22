#nullable enable
namespace SixShaded.FourZeroOne.Token.Defined
{
    public abstract record PureFunction<RArg1, ROut> : Function<RArg1, ROut>
        where RArg1 : class, Res
        where ROut : class, Res
    {
        protected abstract ROut EvaluatePure(RArg1 in1);
        protected PureFunction(IToken<RArg1> in1) : base(in1) { }
        protected sealed override ITask<IOption<ROut>> Evaluate(ITokenContext _, IOption<RArg1> in1)
        {
            var o = in1.CheckNone(out var a) ? new None<ROut>() :
                EvaluatePure(a).AsSome();
            return o.ToCompletedITask();
        }
    }
    public abstract record PureFunction<RArg1, RArg2, ROut> : Function<RArg1, RArg2, ROut>
        where RArg1 : class, Res
        where RArg2 : class, Res
        where ROut : class, Res
    {
        protected abstract ROut EvaluatePure(RArg1 in1, RArg2 in2);
        protected PureFunction(IToken<RArg1> in1, IToken<RArg2> in2) : base(in1, in2) { }
        protected sealed override ITask<IOption<ROut>> Evaluate(ITokenContext _, IOption<RArg1> in1, IOption<RArg2> in2)
        {
            var o = in1.CheckNone(out var a) || in2.CheckNone(out var b) ? new None<ROut>() :
                EvaluatePure(a, b).AsSome();
            return o.ToCompletedITask();
        }
    }
    public abstract record PureFunction<RArg1, RArg2, RArg3, ROut> : Function<RArg1, RArg2, RArg3, ROut>
        where RArg1 : class, Res
        where RArg2 : class, Res
        where RArg3 : class, Res
        where ROut : class, Res
    {
        protected abstract ROut EvaluatePure(RArg1 in1, RArg2 in2, RArg3 in3);
        protected PureFunction(IToken<RArg1> in1, IToken<RArg2> in2, IToken<RArg3> in3) : base(in1, in2, in3) { }
        protected sealed override ITask<IOption<ROut>> Evaluate(ITokenContext _, IOption<RArg1> in1, IOption<RArg2> in2, IOption<RArg3> in3)
        {
            var o = in1.CheckNone(out var a) || in2.CheckNone(out var b) || in3.CheckNone(out var c) ? new None<ROut>() :
                EvaluatePure(a, b, c).AsSome();
            return o.ToCompletedITask();
        }
    }
}