namespace SixShaded.FourZeroOne.Core.Roggis;

using Roggi.Unsafe;

public sealed record MetaFunction<ROut> : Roggi.Defined.ConcreteMetaFunction<ROut>
    where ROut : class, Rog
{
    public required DynamicRoda<MetaFunction<ROut>> SelfRoda { get; init; }
    protected override IRoda<IMetaFunction<ROut>> SelfIdentifierInternal => SelfRoda;
    public override string ToString() => $"#{SelfRoda}{{ => {Korssa} }}";
}

public sealed record MetaFunction<RArg1, ROut> : Roggi.Defined.ConcreteMetaFunction<ROut>
    where RArg1 : class, Rog
    where ROut : class, Rog
{
    public MetaFunction(DynamicRoda<RArg1> addrA)
        : base(addrA)
    { }

    public required DynamicRoda<MetaFunction<RArg1, ROut>> SelfRoda { get; init; }
    public DynamicRoda<RArg1> RodaA => (DynamicRoda<RArg1>)ArgAddresses[0];
    protected override IRoda<IMetaFunction<ROut>> SelfIdentifierInternal => SelfRoda;
    public override string ToString() => $"#{SelfRoda}{{ {RodaA} => {Korssa} }}";
}

public sealed record MetaFunction<RArg1, RArg2, ROut> : Roggi.Defined.ConcreteMetaFunction<ROut>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where ROut : class, Rog
{
    public MetaFunction(
        DynamicRoda<RArg1> addrA,
        DynamicRoda<RArg2> addrB)
        : base(addrA, addrB)
    { }

    public required DynamicRoda<MetaFunction<RArg1, RArg2, ROut>> SelfRoda { get; init; }
    public DynamicRoda<RArg1> RodaA => (DynamicRoda<RArg1>)ArgAddresses[0];
    public DynamicRoda<RArg2> RodaB => (DynamicRoda<RArg2>)ArgAddresses[1];
    protected override IRoda<IMetaFunction<ROut>> SelfIdentifierInternal => SelfRoda;
    public override string ToString() => $"#{SelfRoda}{{ {RodaA}, {RodaB} => {Korssa} }}";
}

public sealed record MetaFunction<RArg1, RArg2, RArg3, ROut> : Roggi.Defined.ConcreteMetaFunction<ROut>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where RArg3 : class, Rog
    where ROut : class, Rog
{
    public MetaFunction(
        DynamicRoda<RArg1> addrA,
        DynamicRoda<RArg2> addrB,
        DynamicRoda<RArg3> addrC)
        : base(addrA, addrB, addrC)
    { }

    public required DynamicRoda<MetaFunction<RArg1, RArg2, RArg3, ROut>> SelfRoda { get; init; }
    public DynamicRoda<RArg1> RodaA => (DynamicRoda<RArg1>)ArgAddresses[0];
    public DynamicRoda<RArg2> RodaB => (DynamicRoda<RArg2>)ArgAddresses[1];
    public DynamicRoda<RArg3> RodaC => (DynamicRoda<RArg3>)ArgAddresses[2];
    protected override IRoda<IMetaFunction<ROut>> SelfIdentifierInternal => SelfRoda;
    public override string ToString() => $"#{SelfRoda}{{ {RodaA}, {RodaB}, {RodaC} => {Korssa} }}";
}

/// <summary>
///     <b>Strictly for internal workings (e.g. Mellsano definitions).</b><br></br>
///     Not for normal use.
/// </summary>
public sealed record OverflowingMetaFunction<RArg1, RArg2, RArg3, RArg4, ROut> : Roggi.Defined.ConcreteMetaFunction<ROut>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where RArg3 : class, Rog
    where RArg4 : class, Rog
    where ROut : class, Rog
{
    public OverflowingMetaFunction(
        DynamicRoda<RArg1> addrA,
        DynamicRoda<RArg2> addrB,
        DynamicRoda<RArg3> addrC,
        DynamicRoda<RArg4> addrD)
        : base(addrA, addrB, addrC, addrD)
    { }

    public required DynamicRoda<OverflowingMetaFunction<RArg1, RArg2, RArg3, RArg4, ROut>> SelfRoda { get; init; }
    public DynamicRoda<RArg1> RodaA => (DynamicRoda<RArg1>)ArgAddresses[0];
    public DynamicRoda<RArg2> RodaB => (DynamicRoda<RArg2>)ArgAddresses[1];
    public DynamicRoda<RArg3> RodaC => (DynamicRoda<RArg3>)ArgAddresses[2];
    public DynamicRoda<RArg4> RodaD => (DynamicRoda<RArg4>)ArgAddresses[3];
    protected override IRoda<IMetaFunction<ROut>> SelfIdentifierInternal => SelfRoda;
    public override string ToString() => $"{SelfRoda}({RodaA}, {RodaB}, {RodaC}, {RodaD})::{{{Korssa}}}";
}