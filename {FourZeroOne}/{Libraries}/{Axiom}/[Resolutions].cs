using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourZeroOne.Libraries.Axiom.Resolutions
{
    using r = Core.Resolutions;
    using Resolution;
    using Core.Resolutions.Actions.Board.Unit;

    namespace Effects
    {
        using Core.Resolutions.Objects.Board;
        public record Slow : Component<Slow, Unit>
        {
            public override ComponentIdentifier<Slow> Identifier => _identifier;
            private static Identifiers.SlowCI _identifier = new();
        }
        namespace Identifiers
        {
            public record SlowCI : ComponentIdentifier<Slow>
            {
                public override string Identity => _identity;
                private static string _identity = "slow";
            }
        }
    }
}