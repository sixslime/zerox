namespace SixShaded.FourZeroOne.Core.Korssas.Memory;

public sealed record Reference<R>(IMemoryAddress<R> address) : Korssa.Defined.Value<R>
    where R : class, Rog
{
    public readonly IMemoryAddress<R> Address = address;

    protected override ITask<IOption<R>> Evaluate(IKorssaContext runtime) => runtime.CurrentMemory.GetObject(Address).ToCompletedITask();
    protected override IOption<string> CustomToString() => $"&{Address}".AsSome();
}