namespace SixShaded.FourZeroOne.Core.Roggis;

using Roggi.Unsafe;

public sealed record MetaFunction<ROut> : Roggi.Defined.ConcreteMetaFunction<ROut>
    where ROut : class, Rog
{
    public required DynamicAddress<MetaFunction<ROut>> SelfAddress { get; init; }
    protected override IMemoryAddress<IMetaFunction<ROut>> SelfIdentifierInternal => SelfAddress;
    public override string ToString() => $"{SelfAddress}()::{{{Korssa}}}";
}

public sealed record MetaFunction<RArg1, ROut> : Roggi.Defined.ConcreteMetaFunction<ROut>
    where RArg1 : class, Rog
    where ROut : class, Rog
{
    public MetaFunction(DynamicAddress<RArg1> addrA)
        : base(addrA)
    { }

    public required DynamicAddress<MetaFunction<RArg1, ROut>> SelfAddress { get; init; }
    public DynamicAddress<RArg1> AddressA => (DynamicAddress<RArg1>)ArgAddresses[0];
    protected override IMemoryAddress<IMetaFunction<ROut>> SelfIdentifierInternal => SelfAddress;
    public override string ToString() => $"{SelfAddress}({AddressA})::{{{Korssa}}}";
}

public sealed record MetaFunction<RArg1, RArg2, ROut> : Roggi.Defined.ConcreteMetaFunction<ROut>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where ROut : class, Rog
{
    public MetaFunction(
        DynamicAddress<RArg1> addrA,
        DynamicAddress<RArg2> addrB)
        : base(addrA, addrB)
    { }

    public required DynamicAddress<MetaFunction<RArg1, RArg2, ROut>> SelfAddress { get; init; }
    public DynamicAddress<RArg1> AddressA => (DynamicAddress<RArg1>)ArgAddresses[0];
    public DynamicAddress<RArg2> AddressB => (DynamicAddress<RArg2>)ArgAddresses[1];
    protected override IMemoryAddress<IMetaFunction<ROut>> SelfIdentifierInternal => SelfAddress;
    public override string ToString() => $"{SelfAddress}({AddressA}, {AddressB})::{{{Korssa}}}";
}

public sealed record MetaFunction<RArg1, RArg2, RArg3, ROut> : Roggi.Defined.ConcreteMetaFunction<ROut>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where RArg3 : class, Rog
    where ROut : class, Rog
{
    public MetaFunction(
        DynamicAddress<RArg1> addrA,
        DynamicAddress<RArg2> addrB,
        DynamicAddress<RArg3> addrC)
        : base(addrA, addrB, addrC)
    { }

    public required DynamicAddress<MetaFunction<RArg1, RArg2, RArg3, ROut>> SelfAddress { get; init; }
    public DynamicAddress<RArg1> AddressA => (DynamicAddress<RArg1>)ArgAddresses[0];
    public DynamicAddress<RArg2> AddressB => (DynamicAddress<RArg2>)ArgAddresses[1];
    public DynamicAddress<RArg3> AddressC => (DynamicAddress<RArg3>)ArgAddresses[2];
    protected override IMemoryAddress<IMetaFunction<ROut>> SelfIdentifierInternal => SelfAddress;
    public override string ToString() => $"{SelfAddress}({AddressA}, {AddressB}, {AddressC})::{{{Korssa}}}";
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
        DynamicAddress<RArg1> addrA,
        DynamicAddress<RArg2> addrB,
        DynamicAddress<RArg3> addrC,
        DynamicAddress<RArg4> addrD)
        : base(addrA, addrB, addrC, addrD)
    { }

    public required DynamicAddress<OverflowingMetaFunction<RArg1, RArg2, RArg3, RArg4, ROut>> SelfAddress { get; init; }
    public DynamicAddress<RArg1> AddressA => (DynamicAddress<RArg1>)ArgAddresses[0];
    public DynamicAddress<RArg2> AddressB => (DynamicAddress<RArg2>)ArgAddresses[1];
    public DynamicAddress<RArg3> AddressC => (DynamicAddress<RArg3>)ArgAddresses[2];
    public DynamicAddress<RArg4> AddressD => (DynamicAddress<RArg4>)ArgAddresses[3];
    protected override IMemoryAddress<IMetaFunction<ROut>> SelfIdentifierInternal => SelfAddress;
    public override string ToString() => $"{SelfAddress}({AddressA}, {AddressB}, {AddressC}, {AddressD})::{{{Korssa}}}";
}