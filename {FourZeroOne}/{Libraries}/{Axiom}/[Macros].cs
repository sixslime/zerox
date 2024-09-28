using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourZeroOne.Libraries.Axiom.Macros
{
    using Token;
    using Macro;
    using t = Core.Tokens;
    using r = Core.Resolutions;
    using FourZeroOne.Proxy;
    using FourZeroOne.Core.Resolutions.Actions.Board.Unit;
    using Core.ProxySyntax;

    namespace GameActions
    {
        using Core.Resolutions.Actions.Board.Unit;
        using Core.Resolutions.Objects.Board;
        public sealed record Move : TwoArg<Unit, Coordinates, PositionChange>
        {
            private static readonly IProxy<Move, PositionChange> _proxy = MakeProxy.Statement<Move, PositionChange>(P =>
            {
                P.
            })
            public Move(IToken<Unit> in1, IToken<Coordinates> in2) : base(in1, in2, _proxy)
            {
            }
        }
    }
}