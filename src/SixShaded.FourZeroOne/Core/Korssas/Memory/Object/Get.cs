namespace SixShaded.FourZeroOne.Core.Korssas.Memory.Object;

public sealed record Get<R> : Korssa.Defined.Function<IMemoryObject<R>, R>
    where R : class, Rog
{
    public Get(IKorssa<IMemoryObject<R>> address) : base(address) { }
    protected override ITask<IOption<R>> Evaluate(IKorssaContext runtime, IOption<IMemoryObject<R>> in1) => in1.RemapAs(x => runtime.CurrentMemory.GetObject(x)).Press().ToCompletedITask();
    protected override IOption<string> CustomToString() => $"*{Arg1}".AsSome();
}