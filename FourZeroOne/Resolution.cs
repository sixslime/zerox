
using System.Collections.Generic;
using Perfection;
using PROTO_ZeroxFour_1.Util;
#nullable enable
namespace FourZeroOne.Resolution
{
    using FourZeroOne.FZOSpec;
    using Handles;
    
    /// <summary>
    /// all inherits must be by a record class.
    /// </summary>
    public interface IResolution
    {
        public IEnumerable<IInstruction> Instructions { get; }
    }
    public interface IInstruction : IResolution
    {
        public IMemory TransformMemory(IMemory context);
        public FZOSpec.IMemoryFZO TransformMemoryUnsafe(FZOSpec.IMemoryFZO memory);
    }
    public interface IComponentIdentifier<in H, out R> : Unsafe.IComponentIdentifier<H>, Unsafe.IComponentIdentifierOf<R> where H : ICompositionType where R : IResolution { }
    // pretty fucking silly bro im not going even to even lie even.
    // DEV: MAY NOT ACTUALLY BE 'out' COMPATIBLE
    public interface ICompositionOf<out C> : Unsafe.ICompositionOf, IResolution where C : ICompositionType
    {
        public IEnumerable<ITiple<Unsafe.IComponentIdentifier, IResolution>> ComponentsUnsafe { get; }
        public ICompositionOf<C> WithComponent<R>(IComponentIdentifier<C, R> identifier, R data) where R : IResolution;
        public ICompositionOf<C> WithoutComponents(IEnumerable<Unsafe.IComponentIdentifier<C>> addresses);
        public IOption<R> GetComponent<R>(IComponentIdentifier<C, R> address) where R : IResolution;
    }
    /// <summary>
    /// Types that implement must be functionally static and have an empty constructor with no init fields. <br></br>
    /// Yup! thats how I'm doing things!
    /// </summary>
    public interface ICompositionType { }
    public interface IDecomposableType<Self, R> : ICompositionType where Self : IDecomposableType<Self, R>, new() where R : class, IResolution
    {
        public Core.Resolutions.Boxed.MetaFunction<ICompositionOf<Self>, R> DecompositionFunction { get; }
    }
    public interface IMulti<out R> : IHasElements<R>, IIndexReadable<int, IOption<R>>, IResolution where R : IResolution
    { }

    public interface IStateAddress<out R> : Unsafe.IStateAddress where R : class, IResolution { }
    public abstract record Instruction : Construct, IInstruction
    {
        public abstract IMemory TransformMemory(IMemory previousState);
        IMemoryFZO IInstruction.TransformMemoryUnsafe(IMemoryFZO memory) => TransformMemory(memory.ToHandle()).InternalValue;
        public override IEnumerable<IInstruction> Instructions => [this];
    }
    public abstract record Construct : IResolution
    {
        public abstract IEnumerable<IInstruction> Instructions { get; }
    }
    // the 'new()' constraint is mega stupid.
    // this is mega stupid.
    public record CompositionOf<C> : NoOp, ICompositionOf<C> where C : ICompositionType, new()
    {
        private PMap<Unsafe.IComponentIdentifier, IResolution> _componentMap { get; init; }
        public IEnumerable<ITiple<Unsafe.IComponentIdentifier, IResolution>> ComponentsUnsafe => _componentMap.Elements;
        public CompositionOf()
        {
            _componentMap = new();
        }
        // UNBELIEVABLY stupid
        public ICompositionOf<C> WithComponent<R>(IComponentIdentifier<C, R> identifier, R data) where R : IResolution => (ICompositionOf<C>)WithComponentsUnsafe(((Unsafe.IComponentIdentifier)identifier, (IResolution)data).Tiple().Yield());
        public Unsafe.ICompositionOf WithComponentsUnsafe(IEnumerable<ITiple<Unsafe.IComponentIdentifier, IResolution>> components)
        {
            return this with { _componentMap = _componentMap.WithEntries(components) };
        }
        public ICompositionOf<C> WithoutComponents(IEnumerable<Unsafe.IComponentIdentifier<C>> addresses)
        {
            return this with { _componentMap = _componentMap.WithoutEntries(addresses) };
        }

        public IOption<R> GetComponent<R>(IComponentIdentifier<C, R> address) where R : IResolution
        {
            return GetComponentUnsafe(address).RemapAs(x => (R)x);
        }
        public IOption<IResolution> GetComponentUnsafe(Unsafe.IComponentIdentifier address)
        {
            return _componentMap.At(address);
        }
        public override string ToString()
        {
            return $"{typeof(C).Namespace!.Split(".")[^1]}.{typeof(C).Name}:{{{string.Join(" ", ComponentsUnsafe.OrderBy(x => x.A.ToString()).Map(x => $"{x.A}={x.B}"))}}}";
        }
        public virtual bool Equals(CompositionOf<C>? other)
        {
            return (other is not null) && ComponentsUnsafe.SequenceEqual(other.ComponentsUnsafe);
        }
        public override int GetHashCode()
        {
            return ComponentsUnsafe.GetHashCode();
        }
    }
    public abstract record NoOp : Construct
    {
        public override IEnumerable<IInstruction> Instructions => [];
    }
    public sealed record DynamicAddress<R> : IStateAddress<R> where R : class, IResolution
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
    public record StaticComponentIdentifier<H, R> : IComponentIdentifier<H, R> where H : ICompositionType where R : class, IResolution
    {
        public string Source { get; }
        public string Identity { get; }
        public StaticComponentIdentifier(string source, string fixedIdentity)
        {
            Source = source;
            Identity = fixedIdentity;
        }
        public override string ToString() => $"{Identity}";
    }
}
namespace FourZeroOne.Resolution.Unsafe
{
    public interface ICompositionOf : IResolution
    {
        public IOption<IResolution> GetComponentUnsafe(IComponentIdentifier address);
        public ICompositionOf WithComponentsUnsafe(IEnumerable<ITiple<IComponentIdentifier, IResolution>> components);
    }
    public interface IComponentIdentifier
    { 
        public string Source { get; }
        public string Identity { get; }
    }
    public interface IComponentIdentifierOf<out R> : IComponentIdentifier where R : IResolution { }
    public interface IComponentIdentifier<in H> : IComponentIdentifier where H : ICompositionType { }
    public interface IStateAddress { }
}