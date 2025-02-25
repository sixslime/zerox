namespace SixShaded.FourZeroOne.Core.Roggis;

using Roggi.Unsafe;

public sealed record MetaFunction<R> : Roggi.Defined.NoOp where R : class, Rog
{
    public required DynamicAddress<MetaFunction<R>> SelfIdentifier { get; init; }
    public required IKorssa<R> Korssa { get; init; }
    public override string ToString() => $"{SelfIdentifier}(){{{Korssa}}}";

    public FZOSpec.EStateImplemented.MetaExecute GenerateMetaExecute() =>
        new()
        {
            Korssa = new Korssas.MetaExecuted<R>(Korssa),
            ObjectWrites = Iter.Over<(Addr, RogOpt)>
                    ((SelfIdentifier, this.AsSome()))
                .Tipled(),
        };
}

public sealed record MetaFunction<RArg1, ROut> : Roggi.Defined.NoOp, IBoxedMetaFunction<ROut>
    where RArg1 : class, Rog
    where ROut : class, Rog
{
    public required DynamicAddress<MetaFunction<RArg1, ROut>> SelfIdentifier { get; init; }
    public required DynamicAddress<RArg1> IdentifierA { get; init; }
    public required IKorssa<ROut> Korssa { get; init; }
    IMemoryAddress<IBoxedMetaFunction<ROut>> IBoxedMetaFunction<ROut>.SelfIdentifier => SelfIdentifier;
    IEnumerable<Addr> IBoxedMetaFunction<ROut>.ArgAddresses => [IdentifierA];
    public override string ToString() => $"{SelfIdentifier}({IdentifierA})::{{{Korssa}}}";

    public FZOSpec.EStateImplemented.MetaExecute GenerateMetaExecute(IOption<RArg1> arg1) =>
        new()
        {
            Korssa = new Korssas.MetaExecuted<ROut>(Korssa),
            ObjectWrites = Iter.Over<(Addr, RogOpt)>
                    ((SelfIdentifier, this.AsSome()), (IdentifierA, arg1))
                .Tipled(),
        };
}

public sealed record MetaFunction<RArg1, RArg2, ROut> : Roggi.Defined.NoOp, IBoxedMetaFunction<ROut>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where ROut : class, Rog
{
    public required DynamicAddress<MetaFunction<RArg1, RArg2, ROut>> SelfIdentifier { get; init; }
    public required DynamicAddress<RArg1> IdentifierA { get; init; }
    public required DynamicAddress<RArg2> IdentifierB { get; init; }
    public required IKorssa<ROut> Korssa { get; init; }
    IMemoryAddress<IBoxedMetaFunction<ROut>> IBoxedMetaFunction<ROut>.SelfIdentifier => SelfIdentifier;
    IEnumerable<Addr> IBoxedMetaFunction<ROut>.ArgAddresses => [IdentifierA, IdentifierB];

    public override string ToString() => $"{SelfIdentifier}({IdentifierA}, {IdentifierB})::{{{Korssa}}}";

    public FZOSpec.EStateImplemented.MetaExecute GenerateMetaExecute(IOption<RArg1> arg1, IOption<RArg2> arg2) =>
        new()
        {
            Korssa = new Korssas.MetaExecuted<ROut>(Korssa),
            ObjectWrites = Iter.Over<(Addr, RogOpt)>
                    ((SelfIdentifier, this.AsSome()), (IdentifierA, arg1), (IdentifierB, arg2))
                .Tipled(),
        };
}

public sealed record MetaFunction<RArg1, RArg2, RArg3, ROut> : Roggi.Defined.NoOp, IBoxedMetaFunction<ROut>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where RArg3 : class, Rog
    where ROut : class, Rog
{
    public required DynamicAddress<MetaFunction<RArg1, RArg2, RArg3, ROut>> SelfIdentifier { get; init; }
    public required DynamicAddress<RArg1> IdentifierA { get; init; }
    public required DynamicAddress<RArg2> IdentifierB { get; init; }
    public required DynamicAddress<RArg3> IdentifierC { get; init; }
    public required IKorssa<ROut> Korssa { get; init; }

    IMemoryAddress<IBoxedMetaFunction<ROut>> IBoxedMetaFunction<ROut>.SelfIdentifier => SelfIdentifier;

    IEnumerable<Addr> IBoxedMetaFunction<ROut>.ArgAddresses => [IdentifierA, IdentifierB, IdentifierC];

    public override string ToString() => $"{SelfIdentifier}({IdentifierA}, {IdentifierB}, {IdentifierC})::{{{Korssa}}}";

    public FZOSpec.EStateImplemented.MetaExecute GenerateMetaExecute(IOption<RArg1> arg1, IOption<RArg2> arg2, IOption<RArg3> arg3) =>
        new()
        {
            Korssa = new Korssas.MetaExecuted<ROut>(Korssa),
            ObjectWrites = Iter.Over<(Addr, RogOpt)>
                    ((SelfIdentifier, this.AsSome()), (IdentifierA, arg1), (IdentifierB, arg2), (IdentifierC, arg3))
                .Tipled(),
        };
}

/// <summary>
///     <b>Strictly for internal workings (e.g. Mellsano definitions).</b><br></br>
///     Not for normal use.
/// </summary>
public sealed record OverflowingMetaFunction<RArg1, RArg2, RArg3, RArg4, ROut> : Roggi.Defined.NoOp, IBoxedMetaFunction<ROut>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where RArg3 : class, Rog
    where RArg4 : class, Rog
    where ROut : class, Rog
{
    public required DynamicAddress<OverflowingMetaFunction<RArg1, RArg2, RArg3, RArg4, ROut>> SelfIdentifier { get; init; }
    public required DynamicAddress<RArg1> IdentifierA { get; init; }
    public required DynamicAddress<RArg2> IdentifierB { get; init; }
    public required DynamicAddress<RArg3> IdentifierC { get; init; }
    public required DynamicAddress<RArg4> IdentifierD { get; init; }
    public required IKorssa<ROut> Korssa { get; init; }

    IMemoryAddress<IBoxedMetaFunction<ROut>> IBoxedMetaFunction<ROut>.SelfIdentifier => SelfIdentifier;

    IEnumerable<Addr> IBoxedMetaFunction<ROut>.ArgAddresses => [IdentifierA, IdentifierB, IdentifierC, IdentifierD];

    public override string ToString() => $"{SelfIdentifier}({IdentifierA}, {IdentifierB}, {IdentifierC}, {IdentifierD})::{{{Korssa}}}";

    public FZOSpec.EStateImplemented.MetaExecute GenerateMetaExecute(IOption<RArg1> arg1, IOption<RArg2> arg2, IOption<RArg3> arg3, IOption<RArg4> arg4) =>
        new()
        {
            Korssa = new Korssas.MetaExecuted<ROut>(Korssa),
            ObjectWrites = Iter.Over<(Addr, RogOpt)>
                    ((SelfIdentifier, this.AsSome()), (IdentifierA, arg1), (IdentifierB, arg2), (IdentifierC, arg3), (IdentifierD, arg4))
                .Tipled(),
        };
}