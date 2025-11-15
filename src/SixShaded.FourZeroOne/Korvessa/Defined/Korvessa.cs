namespace SixShaded.FourZeroOne.Korvessa.Defined;

using Core.Roggis;
using Core.Korssas;
using Core.Syntax;
using Roggi.Unsafe;

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
    public override IMetaFunction<RVal> DefinitionUnsafe => Definition;

}

public abstract record Korvessa<RArg1, ROut>(IKorssa<RArg1> in1) : KorvessaBehavior<ROut>(in1)
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
    public override IMetaFunction<ROut> DefinitionUnsafe => Definition;

}

public abstract record Korvessa<RArg1, RArg2, ROut>(IKorssa<RArg1> in1, IKorssa<RArg2> in2) : KorvessaBehavior<ROut>(in1, in2)
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
    public override IMetaFunction<ROut> DefinitionUnsafe => Definition;

}


public abstract record Korvessa<RArg1, RArg2, RArg3, ROut>(IKorssa<RArg1> in1, IKorssa<RArg2> in2, IKorssa<RArg3> in3) : KorvessaBehavior<ROut>(in1, in2, in3)
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
    public override IMetaFunction<ROut> DefinitionUnsafe => Definition;

}
