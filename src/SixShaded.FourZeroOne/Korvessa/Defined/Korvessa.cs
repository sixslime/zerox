namespace SixShaded.FourZeroOne.Korvessa.Defined;

using Core.Roggis;
using Core.Korssas;

public record Korvessa<RVal> : Korssa.Defined.StateImplementedKorssa<RVal>, IKorvessaSignature<RVal>
    where RVal : class, Rog
{
    public required Func<DynamicRoda<MetaFunction<RVal>>, IKorssa<RVal>> Definition
    {
        init =>
            ConcreteDefinition =
                new DefineMetaFunction<RVal>(value)
                {
                    Captures = [],
                }.ConstructConcreteMetaFunction(KorvessaDummyMemory.INSTANCE);
    }

    public MetaFunction<RVal> ConcreteDefinition { get; private init; } = null!;
    protected override IOption<FZOSpec.EStateImplemented> MakeData(IKorssaContext _) => ConcreteDefinition.ConstructMetaExecute().AsSome();
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

    public required Func<DynamicRoda<MetaFunction<RArg1, ROut>>, DynamicRoda<RArg1>, IKorssa<ROut>> Definition
    {
        init =>
            ConcreteDefinition =
                new DefineMetaFunction<RArg1, ROut>(value)
                {
                    Captures = [],
                }.ConstructConcreteMetaFunction(KorvessaDummyMemory.INSTANCE);
    }

    public MetaFunction<RArg1, ROut> ConcreteDefinition { get; private init; } = null!;
    protected override IOption<FZOSpec.EStateImplemented> MakeData(IKorssaContext _, IOption<RArg1> in1) => ConcreteDefinition.ConstructMetaExecute(in1).AsSome();
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

    public required Func<DynamicRoda<MetaFunction<RArg1, RArg2, ROut>>, DynamicRoda<RArg1>, DynamicRoda<RArg2>, IKorssa<ROut>> Definition
    {
        init =>
            ConcreteDefinition =
                new DefineMetaFunction<RArg1, RArg2, ROut>(value)
                {
                    Captures = [],
                }.ConstructConcreteMetaFunction(KorvessaDummyMemory.INSTANCE);
    }

    public MetaFunction<RArg1, RArg2, ROut> ConcreteDefinition { get; private init; } = null!;
    protected override IOption<FZOSpec.EStateImplemented> MakeData(IKorssaContext _, IOption<RArg1> in1, IOption<RArg2> in2) => ConcreteDefinition.ConstructMetaExecute(in1, in2).AsSome();
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

    public required Func<DynamicRoda<MetaFunction<RArg1, RArg2, RArg3, ROut>>, DynamicRoda<RArg1>, DynamicRoda<RArg2>, DynamicRoda<RArg3>, IKorssa<ROut>> Definition
    {
        init =>
            ConcreteDefinition =
                new DefineMetaFunction<RArg1, RArg2, RArg3, ROut>(value)
                {
                    Captures = [],
                }.ConstructConcreteMetaFunction(KorvessaDummyMemory.INSTANCE);
    }

    public MetaFunction<RArg1, RArg2, RArg3, ROut> ConcreteDefinition { get; private init; } = null!;
    protected override IOption<FZOSpec.EStateImplemented> MakeData(IKorssaContext _, IOption<RArg1> in1, IOption<RArg2> in2, IOption<RArg3> in3) => ConcreteDefinition.ConstructMetaExecute(in1, in2, in3).AsSome();
    protected override IOption<string> CustomToString() => $"{Du.Axodu.Name}.{Du.Identifier}({Arg1}, {Arg2}, {Arg3})".AsSome();
    public required Korvedu Du { get; init; }
    public object[] CustomData { get; init; } = [];
}