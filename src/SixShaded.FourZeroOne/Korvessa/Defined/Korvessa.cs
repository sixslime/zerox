namespace SixShaded.FourZeroOne.Korvessa.Defined;

using Core.Roggis;
using Core.Korssas;
using Core.Syntax;

public abstract record Korvessa<RVal> : KorvessaBehavior<RVal>
    where RVal : class, Rog
{
    protected abstract RecursiveMetaDefinition<RVal> InternalDefinition();

    private static MetaFunction<RVal> MakeConcreteDefinition(RecursiveMetaDefinition<RVal> definition) =>
        new DefineMetaFunction<RVal>(definition)
        {
            Captures = []
        }.ConstructConcreteMetaFunction(KorvessaDummyMemory.INSTANCE);

    private MetaFunction<RVal>? _defCache = null;

    public MetaFunction<RVal> Definition
    {
        get { _defCache ??= MakeConcreteDefinition(InternalDefinition());
            return _defCache;
        }
    }
}

public abstract record Korvessa<RArg1, ROut> : KorvessaBehavior<ROut>
    where RArg1 : class, Rog
    where ROut : class, Rog
{
    protected abstract RecursiveMetaDefinition<RArg1, ROut> InternalDefinition();

    private static MetaFunction<RArg1, ROut> MakeConcreteDefinition(RecursiveMetaDefinition<RArg1, ROut> definition) =>
        new DefineMetaFunction<RArg1, ROut>(definition)
        {
            Captures = []
        }.ConstructConcreteMetaFunction(KorvessaDummyMemory.INSTANCE);

    private MetaFunction<RArg1, ROut>? _defCache = null;

    public MetaFunction<RArg1, ROut> Definition
    {
        get
        {
            _defCache ??= MakeConcreteDefinition(InternalDefinition());
            return _defCache;
        }
    }
}

public abstract record Korvessa<RArg1, RArg2, ROut> : KorvessaBehavior<ROut>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where ROut : class, Rog
{
    protected abstract RecursiveMetaDefinition<RArg1, RArg2, ROut> InternalDefinition();

    private static MetaFunction<RArg1, RArg2, ROut> MakeConcreteDefinition(RecursiveMetaDefinition<RArg1, RArg2, ROut> definition) =>
        new DefineMetaFunction<RArg1, RArg2, ROut>(definition)
        {
            Captures = []
        }.ConstructConcreteMetaFunction(KorvessaDummyMemory.INSTANCE);

    private MetaFunction<RArg1, RArg2, ROut>? _defCache = null;

    public MetaFunction<RArg1, RArg2, ROut> Definition
    {
        get
        {
            _defCache ??= MakeConcreteDefinition(InternalDefinition());
            return _defCache;
        }
    }
}


public abstract record Korvessa<RArg1, RArg2, RArg3, ROut> : KorvessaBehavior<ROut>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where RArg3 : class, Rog
    where ROut : class, Rog
{
    protected abstract RecursiveMetaDefinition<RArg1, RArg2, RArg3, ROut> InternalDefinition();

    private static MetaFunction<RArg1, RArg2, RArg3, ROut> MakeConcreteDefinition(RecursiveMetaDefinition<RArg1, RArg2, RArg3, ROut> definition) =>
        new DefineMetaFunction<RArg1, RArg2, RArg3, ROut>(definition)
        {
            Captures = []
        }.ConstructConcreteMetaFunction(KorvessaDummyMemory.INSTANCE);

    private MetaFunction<RArg1, RArg2, RArg3, ROut>? _defCache = null;

    public MetaFunction<RArg1, RArg2, RArg3, ROut> Definition
    {
        get
        {
            _defCache ??= MakeConcreteDefinition(InternalDefinition());
            return _defCache;
        }
    }
}
