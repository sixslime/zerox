#nullable enable
namespace SixShaded.FourZeroOne.Resolution
{
    public sealed record DynamicAddress<R> : IMemoryAddress<R> where R : class, Res
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
