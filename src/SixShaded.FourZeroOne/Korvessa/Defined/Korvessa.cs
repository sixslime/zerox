namespace SixShaded.FourZeroOne.Korvessa.Defined;

public record Korvessa<RVal> : Korssa.Defined.RuntimeHandledValue<RVal>, IKorvessaSignature<RVal>
    where RVal : class, Rog
{
    public required Core.Roggis.MetaFunction<RVal> Definition { get; init; }
    public object[] CustomData { get; init; } = [];
    public required Korvedu Du { get; init; }
    protected override FZOSpec.EStateImplemented MakeData() => Definition.GenerateMetaExecute();

    protected override IOption<string> CustomToString()
        => $"{Du.Axodu.Name}.{Du.Identifier}()".AsSome();
}

public record Korvessa<RArg1, ROut> : Korssa.Defined.RuntimeHandledFunction<RArg1, ROut>, IKorvessaSignature<RArg1, ROut>
    where RArg1 : class, Rog
    where ROut : class, Rog
{
    public Korvessa(IKorssa<RArg1> in1) : base(in1) { }
    public required Core.Roggis.MetaFunction<RArg1, ROut> Definition { get; init; }
    public required Korvedu Du { get; init; }
    public object[] CustomData { get; init; } = [];
    protected override FZOSpec.EStateImplemented MakeData(RArg1 in1) => Definition.GenerateMetaExecute(in1.AsSome());

    protected override IOption<string> CustomToString()
        => $"{Du.Axodu.Name}.{Du.Identifier}({Arg1})".AsSome();
}

public record Korvessa<RArg1, RArg2, ROut> : Korssa.Defined.RuntimeHandledFunction<RArg1, RArg2, ROut>, IKorvessaSignature<RArg1, RArg2, ROut>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where ROut : class, Rog
{
    public Korvessa(IKorssa<RArg1> in1, IKorssa<RArg2> in2) : base(in1, in2) { }
    public required Core.Roggis.MetaFunction<RArg1, RArg2, ROut> Definition { get; init; }
    public required Korvedu Du { get; init; }
    public object[] CustomData { get; init; } = [];
    protected override FZOSpec.EStateImplemented MakeData(RArg1 in1, RArg2 in2) => Definition.GenerateMetaExecute(in1.AsSome(), in2.AsSome());

    protected override IOption<string> CustomToString()
        => $"{Du.Axodu.Name}.{Du.Identifier}({Arg1}, {Arg2})".AsSome();
}

public record Korvessa<RArg1, RArg2, RArg3, ROut> : Korssa.Defined.RuntimeHandledFunction<RArg1, RArg2, RArg3, ROut>, IKorvessaSignature<RArg1, RArg2, RArg3, ROut>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where RArg3 : class, Rog
    where ROut : class, Rog
{
    public Korvessa(IKorssa<RArg1> in1, IKorssa<RArg2> in2, IKorssa<RArg3> in3) : base(in1, in2, in3) { }
    public required Core.Roggis.MetaFunction<RArg1, RArg2, RArg3, ROut> Definition { get; init; }
    public required Korvedu Du { get; init; }
    public object[] CustomData { get; init; } = [];
    protected override FZOSpec.EStateImplemented MakeData(RArg1 in1, RArg2 in2, RArg3 in3) => Definition.GenerateMetaExecute(in1.AsSome(), in2.AsSome(), in3.AsSome());

    protected override IOption<string> CustomToString()
        => $"{Du.Axodu.Name}.{Du.Identifier}({Arg1}, {Arg2}, {Arg3})".AsSome();
}