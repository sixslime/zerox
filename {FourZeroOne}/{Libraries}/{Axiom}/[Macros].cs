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
    using ProxySyntax;

    namespace GameActions
    {
        using Core.Resolutions.Actions.Board.Unit;
        using Core.Resolutions.Objects.Board;
        public sealed record Move : TwoArg<Unit, Coordinates, PositionChange>
        {
            public static readonly IProxy<Move, PositionChange> PROXY = CoreP.Statement<Move, PositionChange>(P =>
            {
                return P.pOriginalA().pSetPosition(P.pOriginalB());
            });
            public Move(IToken<Unit> in1, IToken<Coordinates> in2) : base(in1, in2, PROXY) { }
        }
    }
}