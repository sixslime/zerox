
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
        public State ChangeState(State context);
        public bool ResEqual(IResolution? other);
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
    public interface IStateTracked<Self> : IResolution where Self : IStateTracked<Self>
    {
        public int UUID { get; }
        public Self GetAtState(State state);
        public State SetAtState(State state);
    }


    public abstract record Operation : Unsafe.Resolution
    {
        protected abstract State UpdateState(State context);
        protected override sealed State ChangeStateInternal(State before) => UpdateState(before);
    }

    public abstract record NoOp : Unsafe.Resolution
    {
        protected override sealed State ChangeStateInternal(State context) => context;
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
        public int UUID => _uuid;

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
            return _components[identifier].NullToNone();
        }

        public StateObject(int id)
        {
            _components = new(x => x.UnsafeIdentifier);
            _uuid = id;
        }
        private readonly int _uuid;
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
    public abstract record Resolution : IResolution
    {
        public virtual bool ResEqual(IResolution? other) => Equals(other);
        public State ChangeState(State before) => ChangeStateInternal(before);
        protected abstract State ChangeStateInternal(State context);
    }
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
}