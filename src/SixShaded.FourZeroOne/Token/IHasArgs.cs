namespace SixShaded.FourZeroOne.Token;

public interface IHasArgs<out RArg1, out ROut> : IToken<ROut>
    where RArg1 : class, Res
    where ROut : class, Res
{
    public IToken<RArg1> Arg1 { get; }
}

public interface IHasArgs<out RArg1, out RArg2, out ROut> : IHasArgs<RArg1, ROut>
    where RArg1 : class, Res
    where RArg2 : class, Res
    where ROut : class, Res
{
    public IToken<RArg2> Arg2 { get; }
}

public interface IHasArgs<out RArg1, out RArg2, out RArg3, out ROut> : IHasArgs<RArg1, RArg2, ROut>
    where RArg1 : class, Res
    where RArg2 : class, Res
    where RArg3 : class, Res
    where ROut : class, Res
{
    public IToken<RArg3> Arg3 { get; }
}