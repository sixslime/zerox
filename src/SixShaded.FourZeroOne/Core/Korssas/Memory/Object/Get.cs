namespace SixShaded.FourZeroOne.Core.Korssas.Memory.Object;

public sealed record Get<R> : Korssa.Defined.Function<IRoveggi<IRovedantu<R>>, R>
    where R : class, Rog
{
    public Get(IKorssa<IRoveggi<IRovedantu<R>>> memoryObj) : base(memoryObj)
    { }

    protected override ITask<IOption<R>> Evaluate(IKorssaContext runtime, IOption<IRoveggi<IRovedantu<R>>> in1) => in1.RemapAs(x => runtime.CurrentMemory.GetObject(x.MemWrapped())).Press().ToCompletedITask();
    protected override IOption<string> CustomToString() => $"*{Arg1}".AsSome();
}