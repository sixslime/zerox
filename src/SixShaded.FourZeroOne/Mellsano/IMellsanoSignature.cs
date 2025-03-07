namespace SixShaded.FourZeroOne.Mellsano;

using Core.Roggis;
using Defined.Proxies;
using Unsafe;

public interface IMellsanoSignature<RVal> : IMellsano<RVal>
    where RVal : class, Rog
{
    public Core.Korssas.DefineMetaFunction<OriginalProxy<RVal>, RVal> Definition { get; }
    public IUllasem<IHasNoArgs<RVal>> Matcher { get; }
}

public interface IMellsanoSignature<RArg1, ROut> : IMellsano<ROut>
    where RArg1 : class, Rog
    where ROut : class, Rog
{
    public Core.Korssas.DefineMetaFunction<OriginalProxy<ROut>, ArgProxy<RArg1>, ROut> Definition { get; }
    public IUllasem<IHasArgs<RArg1, ROut>> Matcher { get; }
}

public interface IMellsanoSignature<RArg1, RArg2, ROut> : IMellsano<ROut>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where ROut : class, Rog
{
    public Core.Korssas.DefineMetaFunction<OriginalProxy<ROut>, ArgProxy<RArg1>, ArgProxy<RArg2>, ROut> Definition { get; }
    public IUllasem<IHasArgs<RArg1, RArg2, ROut>> Matcher { get; }
}

public interface IMellsanoSignature<RArg1, RArg2, RArg3, ROut> : IMellsano<ROut>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where RArg3 : class, Rog
    where ROut : class, Rog
{
    public Core.Korssas.DefineMetaFunction<OriginalProxy<ROut>, ArgProxy<RArg1>, ArgProxy<RArg2>, ArgProxy<RArg3>, ROut> Definition { get; }
    public IUllasem<IHasArgs<RArg1, RArg2, RArg3, ROut>> Matcher { get; }
}