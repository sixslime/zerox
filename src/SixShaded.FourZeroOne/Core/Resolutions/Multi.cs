#nullable enable
using FourZeroOne;

namespace SixShaded.FourZeroOne.Core.Resolutions
{
    public sealed record Multi<R> : Construct, IMulti<R> where R : class, Res
    {
        public IEnumerable<R> Elements => Values.Elements;
        public int Count => Values.Count;
        public override IEnumerable<IInstruction> Instructions => Elements.Map(x => x.Instructions).Flatten();
        public required PSequence<R> Values { get; init; }
        public Updater<PSequence<R>> dValues { init => Values = value(Values); }
        public IOption<R> At(int index)
        {
            try { return Values.At(index).AsSome(); }
            catch { return new None<R>(); }
        }
        public bool Equals(Multi<R>? other)
        {
            return other is not null && Elements.SequenceEqual(other.Elements);
        }
        public override int GetHashCode()
        {
            return Elements.GetHashCode();
        }
        public override string ToString()
        {
            return $"[{string.Join(", ", Elements.Map(x => x.ToString()))}]";
        }
    }
}