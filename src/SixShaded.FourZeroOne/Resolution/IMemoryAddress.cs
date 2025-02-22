#nullable enable
using FourZeroOne;

namespace SixShaded.FourZeroOne.Resolution
{
    using FourZeroOne.FZOSpec;
    using Handles;
    using SixShaded.FourZeroOne;
    using SixShaded.NotRust;
    using SixShaded.SixLib.GFunc;
    public interface IMemoryAddress<out R> where R : class, IResolution { }
}