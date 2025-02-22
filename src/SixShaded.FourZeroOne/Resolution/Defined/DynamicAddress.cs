#nullable enable
using FourZeroOne;

namespace SixShaded.FourZeroOne.Resolution.Defined
{
    using FourZeroOne.FZOSpec;
    using Handles;
    using SixShaded.FourZeroOne;
    using SixShaded.FourZeroOne.Resolution;
    using SixShaded.NotRust;
    using SixShaded.SixLib.GFunc;

    public sealed record DynamicAddress<R> : IMemoryAddress<R> where R : class, IResolution
    {
        public int DynamicId { get; }

        public DynamicAddress()
        {
            DynamicId = _idAssigner++;
        }
        private static int _idAssigner = 0;
        public override string ToString()
        {
            return $"{(DynamicId % 5).ToBase("AOEUI", "")}{(typeof(R).GetHashCode() % 441).ToBase("DHTNSYFPGCRLVWMBXKJQZ".ToLower(), "")}";
        }
    }
}
