namespace SixShaded.FourZeroOne.Core.Korssas.Memory.Object;


public sealed record Get<R> : Korssa.Defined.Function<IRoveggi<Rovedantu<R>>, R>
    where R : class, Rog
{
    public Get(IKorssa<IRoveggi<Rovedantu<R>>> memoryObj) : base(memoryObj)
    { }

    protected override ITask<IOption<R>> Evaluate(IKorssaContext runtime, IOption<IRoveggi<Rovedantu<R>>> in1) => in1.RemapAs(x => runtime.CurrentMemory.GetObject(x.Rovedanggi())).Press().ToCompletedITask();
    protected override IOption<string> CustomToString() => $"*{Arg1}".AsSome();
}