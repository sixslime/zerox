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
    using FourZeroOne.Core.Resolutions.Instructions.Board.Unit;
    using Core.ProxySyntax;
    using ar = Resolutions;
    using ProxySyntax;
    using Resolution;

    namespace GameActions
    {
        using Core.Resolutions.Instructions.Board.Unit;
        using Core.Resolutions.Objects.Board;
        using FourZeroOne.Proxy.Unsafe;

        public sealed record TestMove : TwoArg<Unit, Coordinates, PositionChange>
        {
            public static readonly IProxy<TestMove, PositionChange> PROXY = CoreP.Statement<TestMove, PositionChange>(P =>
            {
                return P.pOriginalA().pSetPosition(P.pOriginalB());
            });
            protected override IProxy<PositionChange> InternalProxy => PROXY;
            public TestMove(IToken<Unit> in1, IToken<Coordinates> in2) : base(in1, in2) { }
        }
        namespace Move
        {
            public sealed record Numerical : ThreeArg<IMulti<Unit>, r.Objects.NumRange, r.Boxed.MetaFunction<Hex, Hex>, r.Multi<ar.GameActions.Move.Numerical>>
            {
                public Numerical(IToken<IMulti<Unit>> units, IToken<r.Objects.NumRange> range, IToken<r.Boxed.MetaFunction<Hex, Hex>> pathModifier) : base(units, range, pathModifier) { }

                private static readonly IProxy<Numerical, ar.GameActions.Move.Numerical>
            }
        }

        // numericalmove(Units, Range, Func<Hex, Hex> modifier)
        //                               ^ Glorping crazy!
    }
    // DEV - instead of changing the checks for the pathfinding, change the hex that is checked!!
}