
using System.Collections.Generic;
using Perfection;
using PROTO_ZeroxFour_1.Util;
#nullable enable
namespace FourZeroOne.Resolution
{

    /// <summary>
    /// all inherits must be by a record class.
    /// </summary>
    public interface IResolution
    {
        public IEnumerable<IInstruction> Instructions { get; }
        public bool ResEqual(IResolution? other);
    }
    public interface IInstruction : IResolution
    {
        public IState ChangeState(IState context);
    }
    public interface IComponentIdentifier<in H, out R> : Unsafe.IComponentIdentifier<H>, Unsafe.IComponentIdentifierOf<R> where H : ICompositionType where R : IResolution { }
    // pretty fucking silly bro im not going even to even lie even.
    public interface IComposition<C> : Unsafe.IComposition, IResolution where C : ICompositionType
    {
        public IComposition<C> WithComponents<R>(IEnumerable<(IComponentIdentifier<C, R>,  R)> components) where R : IResolution;
        public IComposition<C> WithoutComponents(IEnumerable<Unsafe.IComponentIdentifier<C>> addresses);
        public IOption<R> GetComponent<R>(IComponentIdentifier<C, R> address) where R : IResolution;
    }
    /// <summary>
    /// Types that implement must be functionally static and have an empty constructor with no init fields. <br></br>
    /// Yup! thats how I'm doing things!
    /// </summary>
    public interface ICompositionType
    {
        public IResolution InternalResolution { get; }
    }
    public interface IMulti<out R> : IResolution where R : IResolution
    {
        public IEnumerable<R> Values { get; }
        public int Count { get; }
    }

    public interface IStateAddress<out R> : Unsafe.IStateAddress where R : class, IResolution { }
    public abstract record Instruction : Construct, IInstruction
    {
        public abstract IState ChangeState(IState previousState);
        public override IEnumerable<IInstruction> Instructions => [this];
    }
    public abstract record Construct : IResolution
    {
        public abstract IEnumerable<IInstruction> Instructions { get; }
        public virtual bool ResEqual(IResolution? other) => Equals(other);
    }
    // the 'new()' constraint is mega stupid.
    // this is mega stupid.
    public sealed record Composition<C> : Construct, IComposition<C> where C : ICompositionType, new()
    {
        public override IEnumerable<IInstruction> Instructions => _instance.InternalResolution.Instructions;
        public Composition()
        {
            _components = new() { Elements = [] };
            _instance = new();
        }
        public IComposition<C> WithComponents<R>(IEnumerable<(IComponentIdentifier<C, R>, R)> components) where R : IResolution
        {
            return (IComposition<C>)WithComponentsUnsafe(components.Map(x => ((Unsafe.IComponentIdentifier)x.Item1, (IResolution)x.Item2)));
        }
        public Unsafe.IComposition WithComponentsUnsafe(IEnumerable<(Unsafe.IComponentIdentifier, IResolution)> components)
        {
            return this with { _components = _components with { dElements = Q => Q.Also(components) } };
        }
        public IComposition<C> WithoutComponents(IEnumerable<Unsafe.IComponentIdentifier<C>> addresses)
        {
            return this with { _components = _components with { dElements = Q => Q.ExceptBy(addresses, x => x.key) } };
        }

        public IOption<R> GetComponent<R>(IComponentIdentifier<C, R> address) where R : IResolution
        {
            return GetComponentUnsafe(address).RemapAs(x => (R)x);
        }
        public IOption<IResolution> GetComponentUnsafe(Unsafe.IComponentIdentifier address)
        {
            return _components[address];
        }
        private PMap<Unsafe.IComponentIdentifier, IResolution> _components { get; init; }
        private readonly C _instance;
    }
    public abstract record NoOp : Construct
    {
        public override IEnumerable<IInstruction> Instructions => [];
    }
    public sealed record DynamicAddress<R> : IStateAddress<R> where R : class, IResolution
    {
        private readonly int _id;

        public DynamicAddress()
        {
            _id = _idAssigner++;
        }
        private static int _idAssigner = 0;
        public override string ToString()
        {
            return $"{(_id % 5).ToBase("AOEUI", "")}{(typeof(R).GetHashCode() % 441).ToBase("DHTNSYFPGCRLVWMBXKJQZ".ToLower(), "")}";
        }
    }
    public sealed record StaticComponentIdentifier<H, R> : IComponentIdentifier<H, R> where H : IComposition<H> where R : class, IResolution
    {
        public string Source => _source;
        public string Identity => _identifier;
        public StaticComponentIdentifier(string source, string fixedIdentity)
        {
            _source = source;
            _identifier = fixedIdentity;
        }
        private string _identifier;
        private string _source;
        public override string ToString() => $"{Identity}";
    }
}
namespace FourZeroOne.Resolution.Unsafe
{
    public interface IComposition : IResolution
    {
        public IOption<IResolution> GetComponentUnsafe(IComponentIdentifier address);
        public IComposition WithComponentsUnsafe(IEnumerable<(Unsafe.IComponentIdentifier, IResolution)> components);
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