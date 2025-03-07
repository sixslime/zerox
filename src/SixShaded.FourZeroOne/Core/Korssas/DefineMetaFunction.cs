namespace SixShaded.FourZeroOne.Core.Korssas;

using Roggis;

public record DefineMetaFunction<ROut> : Korssa.Defined.MetaFunctionDefinition<ROut, MetaFunction<ROut>>
    where ROut : class, Rog
{
    public override IKorssa<ROut> Korssa { get; }
    public DefineMetaFunction(Func<DynamicAddress<MetaFunction<ROut>>, IKorssa<ROut>> definition)
        : base()
    {
        Korssa = definition(SelfAddress);
    }
    public override MetaFunction<ROut> ConstructConcreteMetaFunction(IMemory memory) =>
        new()
        {
            Korssa = Korssa,
            SelfAddress = SelfAddress,
            CapturedVariables = Captures,
            CapturedMemory = memory,
        };
}

public record DefineMetaFunction<RArg1, ROut> : Korssa.Defined.MetaFunctionDefinition<ROut, MetaFunction<RArg1, ROut>>
    where RArg1 : class, Rog
    where ROut : class, Rog
{
    public DynamicAddress<RArg1> AddressA => (DynamicAddress<RArg1>)ArgAddresses[0];
    public override IKorssa<ROut> Korssa { get; }
    public DefineMetaFunction(Func<DynamicAddress<MetaFunction<RArg1, ROut>>, DynamicAddress<RArg1>, IKorssa<ROut>> definition)
        : base(
        new DynamicAddress<RArg1>())
    {
        Korssa = definition(SelfAddress, AddressA);
    }

    public override MetaFunction<RArg1, ROut> ConstructConcreteMetaFunction(IMemory memory) =>
        new(AddressA)
        {
            Korssa = Korssa,
            SelfAddress = SelfAddress,
            CapturedVariables = Captures,
            CapturedMemory = memory,
        };
}

public record DefineMetaFunction<RArg1, RArg2, ROut> : Korssa.Defined.MetaFunctionDefinition<ROut, MetaFunction<RArg1, RArg2, ROut>>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where ROut : class, Rog
{
    public DynamicAddress<RArg1> AddressA => (DynamicAddress<RArg1>)ArgAddresses[0];
    public DynamicAddress<RArg2> AddressB => (DynamicAddress<RArg2>)ArgAddresses[1];
    public override IKorssa<ROut> Korssa { get; }
    public DefineMetaFunction(Func<DynamicAddress<MetaFunction<RArg1, RArg2, ROut>>, DynamicAddress<RArg1>, DynamicAddress<RArg2>, IKorssa<ROut>> definition)
        : base(
        new DynamicAddress<RArg1>(),
        new DynamicAddress<RArg2>())
    {
        Korssa = definition(SelfAddress, AddressA, AddressB);
    }

    public override MetaFunction<RArg1, RArg2, ROut> ConstructConcreteMetaFunction(IMemory memory) =>
        new(AddressA, AddressB)
        {
            Korssa = Korssa,
            SelfAddress = SelfAddress,
            CapturedVariables = Captures,
            CapturedMemory = memory,
        };
}

public record DefineMetaFunction<RArg1, RArg2, RArg3, ROut> : Korssa.Defined.MetaFunctionDefinition<ROut, MetaFunction<RArg1, RArg2, RArg3, ROut>>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where RArg3 : class, Rog
    where ROut : class, Rog
{
    public DynamicAddress<RArg1> AddressA => (DynamicAddress<RArg1>)ArgAddresses[0];
    public DynamicAddress<RArg2> AddressB => (DynamicAddress<RArg2>)ArgAddresses[1];
    public DynamicAddress<RArg3> AddressC => (DynamicAddress<RArg3>)ArgAddresses[2];
    public override IKorssa<ROut> Korssa { get; }
    public DefineMetaFunction(Func<DynamicAddress<MetaFunction<RArg1, RArg2, RArg3, ROut>>, DynamicAddress<RArg1>, DynamicAddress<RArg2>, DynamicAddress<RArg3>, IKorssa<ROut>> definition)
        : base(
        new DynamicAddress<RArg1>(),
        new DynamicAddress<RArg2>(),
        new DynamicAddress<RArg3>())
    {
        Korssa = definition(SelfAddress, AddressA, AddressB, AddressC);
    }

    public override MetaFunction<RArg1, RArg2, RArg3, ROut> ConstructConcreteMetaFunction(IMemory memory) =>
        new(AddressA, AddressB, AddressC)
        {
            Korssa = Korssa,
            SelfAddress = SelfAddress,
            CapturedVariables = Captures,
            CapturedMemory = memory,
        };
}

public record DefineMetaFunction<RArg1, RArg2, RArg3, RArg4, ROut> : Korssa.Defined.MetaFunctionDefinition<ROut, OverflowingMetaFunction<RArg1, RArg2, RArg3, RArg4, ROut>>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where RArg3 : class, Rog
    where RArg4 : class, Rog
    where ROut : class, Rog
{
    public DynamicAddress<RArg1> AddressA => (DynamicAddress<RArg1>)ArgAddresses[0];
    public DynamicAddress<RArg2> AddressB => (DynamicAddress<RArg2>)ArgAddresses[1];
    public DynamicAddress<RArg3> AddressC => (DynamicAddress<RArg3>)ArgAddresses[2];
    public DynamicAddress<RArg4> AddressD => (DynamicAddress<RArg4>)ArgAddresses[3];
    public override IKorssa<ROut> Korssa { get; }
    public DefineMetaFunction(Func<DynamicAddress<OverflowingMetaFunction<RArg1, RArg2, RArg3, RArg4, ROut>>, DynamicAddress<RArg1>, DynamicAddress<RArg2>, DynamicAddress<RArg3>, DynamicAddress<RArg4>, IKorssa<ROut>> definition)
        : base(
        new DynamicAddress<RArg1>(),
        new DynamicAddress<RArg2>(),
        new DynamicAddress<RArg3>(),
        new DynamicAddress<RArg4>())
    {
        Korssa = definition(SelfAddress, AddressA, AddressB, AddressC, AddressD);
    }

    public override OverflowingMetaFunction<RArg1, RArg2, RArg3, RArg4, ROut> ConstructConcreteMetaFunction(IMemory memory) =>
        new(AddressA, AddressB, AddressC, AddressD)
        {
            Korssa = Korssa,
            SelfAddress = SelfAddress,
            CapturedVariables = Captures,
            CapturedMemory = memory,
        };
}
