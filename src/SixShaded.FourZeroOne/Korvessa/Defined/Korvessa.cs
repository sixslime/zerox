namespace SixShaded.FourZeroOne.Korvessa.Defined;

public record Korvessa<RVal> : Korssa.Defined.StateImplementedKorssa<RVal>, IKorvessaSignature<RVal>
    where RVal : class, Rog
{
    public required Core.Korssas.DefineMetaFunction<RVal> Definition
    {
        init => ConcreteDefinition = value.ConstructConcreteMetaFunction(KorvessaDummyMemory.INSTANCE);
    }

    public Core.Roggis.MetaFunction<RVal> ConcreteDefinition { get; private init; } = null!;
    protected override FZOSpec.EStateImplemented MakeData(IKorssaContext _) => ConcreteDefinition.ConstructMetaExecute();
    protected override IOption<string> CustomToString() => $"{Du.Axodu.Name}.{Du.Identifier}()".AsSome();
    public required Korvedu Du { get; init; }
    public object[] CustomData { get; init; } = [];
}

public record Korvessa<RArg1, ROut> : Korssa.Defined.StateImplementedKorssa<RArg1, ROut>, IKorvessaSignature<RArg1, ROut>
    where RArg1 : class, Rog
    where ROut : class, Rog
{
    public Korvessa(IKorssa<RArg1> in1) : base(in1)
    { }

    public required Core.Korssas.DefineMetaFunction<RArg1, ROut> Definition
    {
        init => ConcreteDefinition = value.ConstructConcreteMetaFunction(KorvessaDummyMemory.INSTANCE);
    }

    public Core.Roggis.MetaFunction<RArg1, ROut> ConcreteDefinition { get; private init; } = null!;
    protected override FZOSpec.EStateImplemented MakeData(IKorssaContext _, IOption<RArg1> in1) => ConcreteDefinition.ConstructMetaExecute(in1);
    protected override IOption<string> CustomToString() => $"{Du.Axodu.Name}.{Du.Identifier}({Arg1})".AsSome();
    public required Korvedu Du { get; init; }
    public object[] CustomData { get; init; } = [];
}

public record Korvessa<RArg1, RArg2, ROut> : Korssa.Defined.StateImplementedKorssa<RArg1, RArg2, ROut>, IKorvessaSignature<RArg1, RArg2, ROut>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where ROut : class, Rog
{
    public Korvessa(IKorssa<RArg1> in1, IKorssa<RArg2> in2) : base(in1, in2)
    { }

    public required Core.Korssas.DefineMetaFunction<RArg1, RArg2, ROut> Definition
    {
        init => ConcreteDefinition = value.ConstructConcreteMetaFunction(KorvessaDummyMemory.INSTANCE);
    }

    public Core.Roggis.MetaFunction<RArg1, RArg2, ROut> ConcreteDefinition { get; private init; } = null!;
    protected override FZOSpec.EStateImplemented MakeData(IKorssaContext _, IOption<RArg1> in1, IOption<RArg2> in2) => ConcreteDefinition.ConstructMetaExecute(in1, in2);
    protected override IOption<string> CustomToString() => $"{Du.Axodu.Name}.{Du.Identifier}({Arg1}, {Arg2})".AsSome();
    public required Korvedu Du { get; init; }
    public object[] CustomData { get; init; } = [];
}

public record Korvessa<RArg1, RArg2, RArg3, ROut> : Korssa.Defined.StateImplementedKorssa<RArg1, RArg2, RArg3, ROut>, IKorvessaSignature<RArg1, RArg2, RArg3, ROut>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where RArg3 : class, Rog
    where ROut : class, Rog
{
    public Korvessa(IKorssa<RArg1> in1, IKorssa<RArg2> in2, IKorssa<RArg3> in3) : base(in1, in2, in3)
    { }

    public required Core.Korssas.DefineMetaFunction<RArg1, RArg2, RArg3, ROut> Definition
    {
        init => ConcreteDefinition = value.ConstructConcreteMetaFunction(KorvessaDummyMemory.INSTANCE);
    }

    public Core.Roggis.MetaFunction<RArg1, RArg2, RArg3, ROut> ConcreteDefinition { get; private init; } = null!;
    protected override FZOSpec.EStateImplemented MakeData(IKorssaContext _, IOption<RArg1> in1, IOption<RArg2> in2, IOption<RArg3> in3) => ConcreteDefinition.ConstructMetaExecute(in1, in2, in3);
    protected override IOption<string> CustomToString() => $"{Du.Axodu.Name}.{Du.Identifier}({Arg1}, {Arg2}, {Arg3})".AsSome();
    public required Korvedu Du { get; init; }
    public object[] CustomData { get; init; } = [];
}