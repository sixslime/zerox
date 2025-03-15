namespace SixShaded.FourZeroOne.Core.Korssas.Memory.Object;

using Roveggi;

public sealed record Insert<R> : Korssa.Defined.PureFunction<IRoveggi<IRovedantu<R>>, R, Roggis.Instructions.Assign<R>>
    where R : class, Rog
{
    public Insert(IKorssa<IRoveggi<IRovedantu<R>>> address, IKorssa<R> obj) : base(address, obj)
    { }

    protected override Roggis.Instructions.Assign<R> EvaluatePure(IRoveggi<IRovedantu<R>> in1, R in2) =>
        new()
        {
            Address = in1.MemWrapped(),
            Subject = in2,
        };

    protected override IOption<string> CustomToString() => $"{Arg1} <== {Arg2}".AsSome();
}