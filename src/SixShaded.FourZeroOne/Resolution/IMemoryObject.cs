#nullable enable
namespace FourZeroOne.Resolution
{
    public interface IMemoryObject<out R> : IMemoryAddress<R>, IResolution where R : class, IResolution { }
    
}