namespace SixShaded.FourZeroOne.Korssa;

public interface IHasArgs<out RArg1, out ROut> : IKorssa<ROut>
    where RArg1 : class, Rog
    where ROut : class, Rog
{
    public IKorssa<RArg1> Arg1 { get; }
}

public interface IHasArgs<out RArg1, out RArg2, out ROut> : IHasArgs<RArg1, ROut>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where ROut : class, Rog
{
    public IKorssa<RArg2> Arg2 { get; }
}

public interface IHasArgs<out RArg1, out RArg2, out RArg3, out ROut> : IHasArgs<RArg1, RArg2, ROut>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where RArg3 : class, Rog
    where ROut : class, Rog
{
    public IKorssa<RArg3> Arg3 { get; }
}