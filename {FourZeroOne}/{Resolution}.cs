
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
    public interface IHasComponents<Self> : IResolution where Self : IHasComponents<Self>
    {
        public PIndexedSet<Unsafe.IComponentIdentifier, Unsafe.IComponentFor<Self>> Components { get; }
    }
    public interface IMulti<out R> : IResolution where R : IResolution
    {
        public IEnumerable<R> Values { get; }
        public int Count { get; }
    }
    public interface IStateTracked<Self> : IHasComponents<Self> where Self : IStateTracked<Self>
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
    public abstract record StateObject<Self> : NoOp, IStateTracked<Self> where Self : StateObject<Self>
    {
        public abstract Self GetAtState(State state);
        public abstract State SetAtState(State state);
        public int UUID => _uuid;
        public PIndexedSet<Unsafe.IComponentIdentifier, Unsafe.IComponentFor<Self>> Components { get; init; }
        public Updater<PIndexedSet<Unsafe.IComponentIdentifier, Unsafe.IComponentFor<Self>>> dComponents { init => Components = value(Components); }

        public StateObject(int id)
        {
            Components = new(x => x.UnsafeIdentifier);
            _uuid = id;
        }
        private readonly int _uuid;
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