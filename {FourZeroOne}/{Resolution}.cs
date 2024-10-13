
using System.Collections.Generic;
using Perfection;

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
        public State ChangeState(State context);
    }
    public interface IComponent<C, H> : Unsafe.IComponent<C>, Unsafe.IComponentFor<H> where C : IComponent<C, H> where H : IHasComponents<H> { }
    public interface IComponentIdentifier<C> : Unsafe.IComponentIdentifier where C : Unsafe.IComponent<C> { }
    public interface IHasComponents<Self> : IStateTracked<Self> where Self : IHasComponents<Self>
    {
        public Self WithComponents(IEnumerable<Unsafe.IComponentFor<Self>> component);
        public Self WithoutComponents(IEnumerable<Unsafe.IComponentIdentifier> identifiers);
        public IOption<C> GetComponent<C>(IComponentIdentifier<C> identifier) where C : class, IComponent<C, Self>;
        public IOption<Unsafe.IComponentFor<Self>> GetComponentUnsafe(Unsafe.IComponentIdentifier identifier);
    }
    public interface IMulti<out R> : IResolution where R : IResolution
    {
        public IEnumerable<R> Values { get; }
        public int Count { get; }
    }
    public interface IStateTracked<Self> : Unsafe.IStateTracked where Self : IStateTracked<Self>
    {
        public Self GetAtState(State state);
    }

    public abstract record Instruction : Composition, IInstruction
    {
        public abstract State ChangeState(State previousState);
        public override IEnumerable<IInstruction> Instructions => [this];
    }
    public abstract record Composition : IResolution
    {
        public abstract IEnumerable<IInstruction> Instructions { get; }
        public virtual bool ResEqual(IResolution? other) => Equals(other);
    }

    public abstract record NoOp : Composition
    {
        public override IEnumerable<IInstruction> Instructions => [];
    }

    public abstract record ComponentIdentifier<C> : NoOp, IComponentIdentifier<C> where C : Unsafe.IComponent<C>
    {
        public abstract string Identity { get; }
        public virtual bool Equals(ComponentIdentifier<C>? other)
        {
            return other is not null && other.Identity == Identity;
        }
        public override int GetHashCode()
        {
            return Identity.GetHashCode();
        }
    }
    public abstract record Component<C, H> : NoOp, IComponent<C, H> where C : Component<C, H> where H : IHasComponents<H>
    {
        public abstract IComponentIdentifier<C> Identifier { get; }
        public Unsafe.IComponentIdentifier UnsafeIdentifier => Identifier;
    }
    public abstract record StateObject<Self> : NoOp, IHasComponents<Self> where Self : StateObject<Self>
    {
        public abstract Self GetAtState(State state);
        public abstract State SetAtState(State state);
        public abstract State RemoveAtState(State state);
        public Unsafe.IStateTracked GetAtStateUnsafe(State state) => GetAtState(state);

        public Self WithComponents(IEnumerable<Unsafe.IComponentFor<Self>> components)
        {
            return (Self)(this with { _components = _components with { dElements = Q => Q.Also(components) } });
        }
        public Self WithoutComponents(IEnumerable<Unsafe.IComponentIdentifier> identifiers)
        {
            //works ig?
            return (Self)(this with { _components = _components with { dElements = Q => Q.ExceptBy(identifiers, _components.IndexGenerator) } });
        }
        public IOption<C> GetComponent<C>(IComponentIdentifier<C> identifier) where C : class, IComponent<C, Self>
        {
            return GetComponentUnsafe(identifier).RemapAs(x => (C)x);
        }
        public IOption<Unsafe.IComponentFor<Self>> GetComponentUnsafe(Unsafe.IComponentIdentifier identifier)
        {
            return _components[identifier];
        }

        public StateObject()
        {
            _components = new(x => x.UnsafeIdentifier) { Elements = [] };
        }
        private PIndexedSet<Unsafe.IComponentIdentifier, Unsafe.IComponentFor<Self>> _components { get; init; }
    }
    namespace Board
    {
        public interface IPositioned : IResolution
        {
            public Core.Resolutions.Objects.Board.Coordinates Position { get; }
        }
        
    }
}
namespace FourZeroOne.Resolution.Unsafe
{
    //not actually unsafe, just here because you should either extend 'Operation' or 'NoOp'.
    
    public interface IComponentFor<H> : IResolution where H : IHasComponents<H>
    {
        public IComponentIdentifier UnsafeIdentifier { get; }
    }
    public interface IComponent<C> : IResolution where C : IComponent<C>
    {
        public IComponentIdentifier<C> Identifier { get; }
    }
    public interface IComponentIdentifier : IResolution
    {
        public string Identity { get; }
    }
    public interface IStateTracked : IResolution
    {
        public IStateTracked GetAtStateUnsafe(State state);
        public State SetAtState(State state);
        public State RemoveAtState(State state);
    }
}