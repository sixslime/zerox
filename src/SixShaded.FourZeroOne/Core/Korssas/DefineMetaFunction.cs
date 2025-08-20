namespace SixShaded.FourZeroOne.Core.Korssas;

using Roggis;
using Syntax;

public record DefineMetaFunction<ROut>
    : Korssa.Defined.MetaFunctionDefinition<ROut, MetaFunction<ROut>>
    where ROut : class, Rog
{
    public DefineMetaFunction(RecursiveMetaDefinition<ROut> definition)
    {
        Korssa = definition(SelfRoda);
    }

    public override IKorssa<ROut> Korssa { get; }

    public override MetaFunction<ROut> ConstructConcreteMetaFunction(IMemory memory) =>
        new()
        {
            Korssa = Korssa,
            SelfRoda = SelfRoda,
            CapturedVariables = Captures,
            CapturedMemory = memory,
        };
}

public record DefineMetaFunction<RArg1, ROut>
    : Korssa.Defined.MetaFunctionDefinition<ROut, MetaFunction<RArg1, ROut>>
    where RArg1 : class, Rog
    where ROut : class, Rog
{
    public DefineMetaFunction(RecursiveMetaDefinition<RArg1, ROut> definition)
        : base(new DynamicRoda<RArg1>())
    {
        Korssa = definition(SelfRoda, RodaA);
    }

    public DynamicRoda<RArg1> RodaA => (DynamicRoda<RArg1>)ArgAddresses[0];
    public override IKorssa<ROut> Korssa { get; }

    public override MetaFunction<RArg1, ROut> ConstructConcreteMetaFunction(IMemory memory) =>
        new(RodaA)
        {
            Korssa = Korssa,
            SelfRoda = SelfRoda,
            CapturedVariables = Captures,
            CapturedMemory = memory,
        };
}

public record DefineMetaFunction<RArg1, RArg2, ROut>
    : Korssa.Defined.MetaFunctionDefinition<ROut, MetaFunction<RArg1, RArg2, ROut>>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where ROut : class, Rog
{
    public DefineMetaFunction(RecursiveMetaDefinition<RArg1, RArg2, ROut> definition)
        : base(
            new DynamicRoda<RArg1>(),
            new DynamicRoda<RArg2>())
    {
        Korssa = definition(SelfRoda, RodaA, RodaB);
    }

    public DynamicRoda<RArg1> RodaA => (DynamicRoda<RArg1>)ArgAddresses[0];
    public DynamicRoda<RArg2> RodaB => (DynamicRoda<RArg2>)ArgAddresses[1];
    public override IKorssa<ROut> Korssa { get; }

    public override MetaFunction<RArg1, RArg2, ROut> ConstructConcreteMetaFunction(IMemory memory) =>
        new(RodaA, RodaB)
        {
            Korssa = Korssa,
            SelfRoda = SelfRoda,
            CapturedVariables = Captures,
            CapturedMemory = memory,
        };
}

public record DefineMetaFunction<RArg1, RArg2, RArg3, ROut>
    : Korssa.Defined.MetaFunctionDefinition<ROut, MetaFunction<RArg1, RArg2, RArg3, ROut>>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where RArg3 : class, Rog
    where ROut : class, Rog
{
    public DefineMetaFunction(RecursiveMetaDefinition<RArg1, RArg2, RArg3, ROut> definition)
        : base(
            new DynamicRoda<RArg1>(),
            new DynamicRoda<RArg2>(),
            new DynamicRoda<RArg3>())
    {
        Korssa = definition(SelfRoda, RodaA, RodaB, RodaC);
    }

    public DynamicRoda<RArg1> RodaA => (DynamicRoda<RArg1>)ArgAddresses[0];
    public DynamicRoda<RArg2> RodaB => (DynamicRoda<RArg2>)ArgAddresses[1];
    public DynamicRoda<RArg3> RodaC => (DynamicRoda<RArg3>)ArgAddresses[2];
    public override IKorssa<ROut> Korssa { get; }

    public override MetaFunction<RArg1, RArg2, RArg3, ROut> ConstructConcreteMetaFunction(IMemory memory) =>
        new(RodaA, RodaB, RodaC)
        {
            Korssa = Korssa,
            SelfRoda = SelfRoda,
            CapturedVariables = Captures,
            CapturedMemory = memory,
        };
}

// OverflowingMetaFunction likely can't use RecursiveMetaDefinition
// because your delegate types stop at 3 args.
public record DefineMetaFunction<RArg1, RArg2, RArg3, RArg4, ROut>
    : Korssa.Defined.MetaFunctionDefinition<ROut, OverflowingMetaFunction<RArg1, RArg2, RArg3, RArg4, ROut>>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where RArg3 : class, Rog
    where RArg4 : class, Rog
    where ROut : class, Rog
{
    public DefineMetaFunction(Func<DynamicRoda<OverflowingMetaFunction<RArg1, RArg2, RArg3, RArg4, ROut>>, DynamicRoda<RArg1>, DynamicRoda<RArg2>, DynamicRoda<RArg3>, DynamicRoda<RArg4>, IKorssa<ROut>> definition)
        : base(
            new DynamicRoda<RArg1>(),
            new DynamicRoda<RArg2>(),
            new DynamicRoda<RArg3>(),
            new DynamicRoda<RArg4>())
    {
        Korssa = definition(SelfRoda, RodaA, RodaB, RodaC, RodaD);
    }

    public DynamicRoda<RArg1> RodaA => (DynamicRoda<RArg1>)ArgAddresses[0];
    public DynamicRoda<RArg2> RodaB => (DynamicRoda<RArg2>)ArgAddresses[1];
    public DynamicRoda<RArg3> RodaC => (DynamicRoda<RArg3>)ArgAddresses[2];
    public DynamicRoda<RArg4> RodaD => (DynamicRoda<RArg4>)ArgAddresses[3];
    public override IKorssa<ROut> Korssa { get; }

    public override OverflowingMetaFunction<RArg1, RArg2, RArg3, RArg4, ROut> ConstructConcreteMetaFunction(IMemory memory) =>
        new(RodaA, RodaB, RodaC, RodaD)
        {
            Korssa = Korssa,
            SelfRoda = SelfRoda,
            CapturedVariables = Captures,
            CapturedMemory = memory,
        };
}
