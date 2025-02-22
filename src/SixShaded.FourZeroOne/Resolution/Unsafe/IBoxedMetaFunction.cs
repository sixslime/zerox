#nullable enable
namespace SixShaded.FourZeroOne.Resolution.Unsafe
{
    using FourZeroOne.FZOSpec;
    using Handles;
    using SixShaded.FourZeroOne;
    using SixShaded.NotRust;
    using SixShaded.SixLib.GFunc;

    public interface IBoxedMetaFunction<out R> : IResolution
        where R : class, IResolution
    {
        public IToken<R> Token { get; }
        public IMemoryAddress<IBoxedMetaFunction<R>> SelfIdentifier { get; }
        public IEnumerable<IMemoryAddress<IResolution>> ArgAddresses { get; }
    }
}
