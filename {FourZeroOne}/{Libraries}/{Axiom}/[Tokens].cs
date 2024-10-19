
using Perfection;
using System.Collections.Generic;
namespace FourZeroOne.Libraries.Axiom.Tokens
{
    using r = Core.Resolutions;
    using ro = Core.Resolutions.Objects;
    using ar = Resolutions;
    using ax = Resolutions.Objects;
    using t = Core.Tokens;
    using ResObj = Resolution.IResolution;
    using Token;
    
    namespace Hex
    {
        namespace Position
        {
            public sealed record Add : Function<ax.Hex.Position, ax.Hex.Position, ax.Hex.Position>
        }
    }
}