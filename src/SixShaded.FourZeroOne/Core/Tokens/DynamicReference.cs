namespace SixShaded.FourZeroOne.Core.Tokens;

public sealed record DynamicReference<R> : Token.Defined.Value<R> where R : class, Res
{
    private readonly IMemoryAddress<R> _referenceAddress;

    public DynamicReference(IMemoryAddress<R> referenceAddress)
    {
        _referenceAddress = referenceAddress;
    }

    protected override ITask<IOption<R>> Evaluate(ITokenContext runtime) => runtime.CurrentMemory.GetObject(_referenceAddress).ToCompletedITask();
    protected override IOption<string> CustomToString() => $"&{_referenceAddress}".AsSome();
}