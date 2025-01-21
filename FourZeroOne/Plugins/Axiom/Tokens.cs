
using Perfection;
using System.Collections.Generic;
namespace FourZeroOne.Plugins.Axiom.Tokens
{
    using r = Core.Resolutions;
    using ro = Core.Resolutions.Objects;
    using ar = Resolutions;
    using ax = Resolutions.GameObjects;
    using t = Core.Tokens;
    using ResObj = Resolution.IResolution;
    using Token;
    using Resolution;
    using FourZeroOne.Plugins.Axiom.Resolutions.Structures;

    namespace Hex
    {
        namespace Position
        {
            /*
            public sealed record Offset : PureFunction<IMulti<ax.Hex.Position>, ax.Hex.Position, HexArea>
            {
                public Offset(IToken<IMulti<ax.Hex.Position>> area, IToken<ax.Hex.Position> offset) : base(area, offset) { }
                protected override HexArea EvaluatePure(IMulti<ax.Hex.Position> in1, ax.Hex.Position in2)
                {
                    return new() { Center = in2, Offsets = new() { Elements = in1.ValueSequence } };
                }
            }
            */
        } 
    }
}