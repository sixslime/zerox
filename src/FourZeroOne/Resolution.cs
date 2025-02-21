using SixLib.GFunc;
#nullable enable
namespace FourZeroOne.Resolution
{
    using FourZeroOne.FZOSpec;
    using Handles;
    using SixShaded.NotRust;

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
    public interface IComponentIdentifier<in C, in R> : Unsafe.IComponentIdentifier<C> where C : ICompositionType where R : IResolution { }

    // pretty fucking silly bro im not going even to even lie even.
    public interface ICompositionOf<out C> : IResolution where C : ICompositionType
    {
        public IEnumerable<ITiple<Unsafe.IComponentIdentifier, IResolution>> ComponentsUnsafe { get; }
        public ICompositionOf<C> WithComponent<R>(IComponentIdentifier<C, R> identifier, R data) where R : IResolution;
        public ICompositionOf<C> WithComponentsUnsafe(IEnumerable<ITiple<Unsafe.IComponentIdentifier<C>, IResolution>> components);
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

    public interface IMemoryObject<out R> : IMemoryAddress<R>, IResolution where R : class, IResolution { }
    public interface IMemoryAddress<out R> where R : class, IResolution { }
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
        private PMap<Unsafe.IComponentIdentifier<C>, IResolution> _componentMap { get; init; }
        public IEnumerable<ITiple<Unsafe.IComponentIdentifier, IResolution>> ComponentsUnsafe => _componentMap.Elements;
        public CompositionOf()
        {
            _componentMap = new();
        }
        // UNBELIEVABLY stupid
        public ICompositionOf<C> WithComponent<R>(IComponentIdentifier<C, R> identifier, R data) where R : IResolution
        {
            return this with { _componentMap = _componentMap.WithEntries((identifier.IsA<Unsafe.IComponentIdentifier<C>>(), data.IsA<IResolution>()).Tiple()) };
        }
        public ICompositionOf<C> WithComponentsUnsafe(IEnumerable<ITiple<Unsafe.IComponentIdentifier<C>, IResolution>> components)
        {
            return this with { _componentMap = _componentMap.WithEntries(components) };
        }
        public ICompositionOf<C> WithoutComponents(IEnumerable<Unsafe.IComponentIdentifier<C>> addresses)
        {
            return this with { _componentMap = _componentMap.WithoutEntries(addresses) };
        }

        public IOption<R> GetComponent<R>(IComponentIdentifier<C, R> address) where R : IResolution
        {
            return _componentMap.At(address).RemapAs(x => (R)x);
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
    public record StaticComponentIdentifier<H, R> : IComponentIdentifier<H, R> where H : ICompositionType where R : class, IResolution
    {
        public string Package { get; }
        public string Identity { get; }
        public StaticComponentIdentifier(string source, string fixedIdentity)
        {
            Package = source;
            Identity = fixedIdentity;
        }
        public override string ToString() => $"{Identity}";
    }

    namespace Unsafe
    {
        public interface IComponentIdentifier<in C> : IComponentIdentifier where C : ICompositionType { }
        public interface IComponentIdentifier
        {
            public string Identity { get; }
            public string Package { get; }
        }
        public interface IBoxedMetaFunction<out R> : IResolution
            where R : class, IResolution
        {
            public Token.IToken<R> Token { get; }
            public IMemoryAddress<IBoxedMetaFunction<R>> SelfIdentifier { get; }
            public IEnumerable<IMemoryAddress<IResolution>> ArgAddresses { get; }
        }
    }
}