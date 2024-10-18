
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
    public interface IComponentIdentifier<in H, out R> : Unsafe.IComponentIdentifier<H>, Unsafe.IComponentIdentifierOf<R> where H : IComposition<H> where R : IResolution { }
    public interface IComposition<Self> : IResolution where Self : IComposition<Self>
    {
        public Self WithComponents<R>(IEnumerable<(IComponentIdentifier<Self, R>,  R)> components) where R : IResolution;
        public Self WithoutComponents(IEnumerable<Unsafe.IComponentIdentifier<Self>> addresses);
        public IOption<R> GetComponent<R>(IComponentIdentifier<Self, R> address) where R : IResolution;
    }
    public interface IMulti<out R> : IResolution where R : IResolution
    {
        public IEnumerable<R> Values { get; }
        public int Count { get; }
    }

    public interface IStateAddress<out R> : Unsafe.IStateAddress where R : IResolution { }
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
        public string Identity => _identifier;
        public StaticComponentIdentifier(string fixedIdentity)
        {
            _identifier = fixedIdentity;
        }
        private string _identifier;
    }
}
namespace FourZeroOne.Resolution.Unsafe
{
    //not actually unsafe, just here because you should either extend 'Operation' or 'NoOp'.
    
    public interface IComposition : IResolution
    {
        public IOption<IResolution> GetComponentUnsafe(IComponentIdentifier address);
    }
    public interface IComponentIdentifier
    { 
        public string Identity { get; }
    }
    public interface IComponentIdentifierOf<out R> : IComponentIdentifier where R : IResolution { }
    public interface IComponentIdentifier<in H> : IComponentIdentifier where H : IComposition<H> { }
    public interface IStateAddress { }
}