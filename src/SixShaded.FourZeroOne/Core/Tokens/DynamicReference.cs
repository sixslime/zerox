#nullable enable
namespace SixShaded.FourZeroOne.Core.Tokens
{
    public sealed record DynamicReference<R> : Token.Defined.Value<R> where R : class, Res
    {
        public DynamicReference(IMemoryAddress<R> referenceAddress)
        {
            _referenceAddress = referenceAddress;
        }

        protected override ITask<IOption<R>> Evaluate(ITokenContext runtime)
        {
            return runtime.CurrentMemory.GetObject(_referenceAddress).ToCompletedITask();
        }
        protected override IOption<string> CustomToString() => $"&{_referenceAddress}".AsSome();

        private readonly IMemoryAddress<R> _referenceAddress;
    }
}