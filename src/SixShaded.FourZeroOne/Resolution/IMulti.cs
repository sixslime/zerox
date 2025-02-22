#nullable enable
using FourZeroOne;

namespace SixShaded.FourZeroOne.Resolution
{
    using FourZeroOne.FZOSpec;
    using Handles;
    using SixShaded.FourZeroOne;
    using SixShaded.NotRust;
    using SixShaded.SixLib.GFunc;

    public interface IMulti<out R> : IHasElements<R>, IIndexReadable<int, IOption<R>>, IResolution where R : IResolution
    { }

}