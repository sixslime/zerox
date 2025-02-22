#nullable enable
using FourZeroOne;

namespace SixShaded.FourZeroOne.Core.Tokens
{
    public sealed record DynamicReference<R> : Value<R> where R : class, ResObj
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