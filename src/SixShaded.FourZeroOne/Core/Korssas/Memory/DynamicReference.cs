namespace SixShaded.FourZeroOne.Core.Korssas.Memory;

public sealed record DynamicReference<R> : Korssa.Defined.Value<R> where R : class, Rog
{
    private readonly IMemoryAddress<R> _referenceAddress;

    public DynamicReference(IMemoryAddress<R> referenceAddress)
    {
        _referenceAddress = referenceAddress;
    }

    protected override ITask<IOption<R>> Evaluate(IKorssaContext runtime) => runtime.CurrentMemory.GetObject(_referenceAddress).ToCompletedITask();
    protected override IOption<string> CustomToString() => $"&{_referenceAddress}".AsSome();
}