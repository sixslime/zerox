namespace SixShaded.FourZeroOne.Core.Korssas.Memory.Object;

using Roveggi;

public sealed record Insert<R> : Korssa.Defined.Function<IRoveggi<IRovedantu<R>>, R, Roggis.Instructions.Assign<R>>
    where R : class, Rog
{
    public Insert(IKorssa<IRoveggi<IRovedantu<R>>> address, IKorssa<R> obj) : base(address, obj)
    { }

    protected override ITask<IOption<Roggis.Instructions.Assign<R>>> Evaluate(IKorssaContext _, IOption<IRoveggi<IRovedantu<R>>> in1, IOption<R> in2) =>
        in1.RemapAs(
            address =>
                new Roggis.Instructions.Assign<R>()
                {
                    Address = address.MemWrapped(),
                    Subject = in2,
                })
            .ToCompletedITask();

    protected override IOption<string> CustomToString() => $"{Arg1} <== {Arg2}".AsSome();
}