
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixShaded.FourZeroOne.Core.Resolutions
{
    // the 'new()' constraint is mega stupid.
    // this is mega stupid.
    // update from HQ, its a regular amount of stupid.
    public record CompositionOf<C> : Resolution.Defined.NoOp, ICompositionOf<C> where C : ICompositionType, new()
    {
        private PMap<Resolution.Unsafe.IComponentIdentifier<C>, Res> _componentMap { get; init; }
        public IEnumerable<ITiple<Resolution.Unsafe.IComponentIdentifier, Res>> ComponentsUnsafe => _componentMap.Elements;
        public CompositionOf()
        {
            _componentMap = new();
        }
        // UNBELIEVABLY stupid
        public ICompositionOf<C> WithComponent<R>(IComponentIdentifier<C, R> identifier, R data) where R : class, Res
        {
            return this with { _componentMap = _componentMap.WithEntries((identifier.IsA<Resolution.Unsafe.IComponentIdentifier<C>>(), data.IsA<Res>()).Tiple()) };
        }
        public ICompositionOf<C> WithComponentsUnsafe(IEnumerable<ITiple<Resolution.Unsafe.IComponentIdentifier<C>, Res>> components)
        {
            return this with { _componentMap = _componentMap.WithEntries(components) };
        }
        public ICompositionOf<C> WithoutComponents(IEnumerable<Resolution.Unsafe.IComponentIdentifier<C>> addresses)
        {
            return this with { _componentMap = _componentMap.WithoutEntries(addresses) };
        }

        public IOption<R> GetComponent<R>(IComponentIdentifier<C, R> address) where R : class, Res
        {
            return _componentMap.At(address).RemapAs(x => (R)x);
        }
        public override string ToString()
        {
            return $"{typeof(C).Namespace!.Split(".")[^1]}.{typeof(C).Name}:{{{string.Join(" ", ComponentsUnsafe.OrderBy(x => x.A.ToString()).Map(x => $"{x.A}={x.B}"))}}}";
        }
        public virtual bool Equals(CompositionOf<C>? other)
        {
            return other is not null && ComponentsUnsafe.SequenceEqual(other.ComponentsUnsafe);
        }
        public override int GetHashCode()
        {
            return ComponentsUnsafe.GetHashCode();
        }
    }
}
