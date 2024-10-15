
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
        public IState ChangeState(IState context);
    }
    public interface IAddress<in H, out R> : Unsafe.IAddress<H>, Unsafe.IAddressOf<R> where H : IComposition<H> where R : IResolution { }
    public interface IComposition<Self> : IResolution where Self : IComposition<Self>
    {
        public Self WithComponents<R>(IEnumerable<(IAddress<Self, R>,  R)> components) where R : IResolution;
        public Self WithoutComponents(IEnumerable<Unsafe.IAddress> addresses);
        public IOption<R> GetComponent<R>(IAddress<Self, R> address) where R : IResolution;
    }
    public interface IMulti<out R> : IResolution where R : IResolution
    {
        public IEnumerable<R> Values { get; }
        public int Count { get; }
    }

    public interface IStateAddress<out R> : IResolution where R : IResolution { }
    public abstract record Instruction : Resolution, IInstruction
    {
        public abstract IState ChangeState(IState previousState);
        public override IEnumerable<IInstruction> Instructions => [this];
    }
    public abstract record Resolution : IResolution
    {
        public abstract IEnumerable<IInstruction> Instructions { get; }
        public virtual bool ResEqual(IResolution? other) => Equals(other);
    }

    public abstract record NoOp : Resolution
    {
        public override IEnumerable<IInstruction> Instructions => [];
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
    
    public interface IComposition : IResolution
    {
        public IOption<IResolution> GetComponentUnsafe(IAddress address);
    }
    public interface IAddress : IResolution
    { 
        public string Identity { get; }
    }
    public interface IAddressOf<out R> : IAddress where R : IResolution { }
    public interface IAddress<in H> : IAddress where H : IComposition<H> { }
    public interface IStateAddress
    {
        public string Identity { get; }
    }
}