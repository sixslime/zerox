namespace SixShaded.FourZeroOne.Korvessa;

using Core.Roggis;

public interface IKorvessa<RVal> : Unsafe.IKorvessa<RVal>, IHasNoArgs<RVal>
    where RVal : class, Rog
{
    public MetaFunction<RVal> Definition { get; }
}

public interface IKorvessa<RArg1, ROut> : Unsafe.IKorvessa<ROut>, IHasArgs<RArg1, ROut>
    where RArg1 : class, Rog
    where ROut : class, Rog
{
    public MetaFunction<RArg1, ROut> Definition { get; }
}

public interface IKorvessa<RArg1, RArg2, ROut> : Unsafe.IKorvessa<ROut>, IHasArgs<RArg1, RArg2, ROut>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where ROut : class, Rog
{
    public MetaFunction<RArg1, RArg2, ROut> Definition { get; }
}

public interface IKorvessa<RArg1, RArg2, RArg3, ROut> : Unsafe.IKorvessa<ROut>, IHasArgs<RArg1, RArg2, RArg3, ROut>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where RArg3 : class, Rog
    where ROut : class, Rog
{
    public MetaFunction<RArg1, RArg2, RArg3, ROut> Definition { get; }
}