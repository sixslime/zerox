
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
    public interface IComponent<in R> : Unsafe.IComponent where R : IResolution
    {

    }
    public interface IMulti<out R> : IResolution where R : IResolution
    {
        public IEnumerable<R> Values { get; }
        public int Count { get; }
    }
    public interface IStateTracked<S> : IResolution where S : IStateTracked<S>
    {
        public int UUID { get; }
        public S GetAtState(State state);
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
    public interface IComponent : IResolution
    {

    }
}