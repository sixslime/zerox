using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourZeroOne.Libraries.Axiom.Components
{
    using r = Core.Resolutions;
    using Resolution;
    using Core.Resolutions.Actions.Board.Unit;
    using b = Core.Resolutions.Objects.Board;

    namespace Unit
    {
        namespace Effects
        {
            namespace Slow
            {
                public record Component : Component<Component, b.Unit>
                {
                    public override ComponentIdentifier<Component> Identifier => _identifier;
                    private static Identifier _identifier = new();
                }
                public record Identifier : ComponentIdentifier<Component>
                {
                    public override string Identity => _identity;
                    private static string _identity = "unit.slow";
                }
            }
        }
    }
    namespace Hex
    {
        namespace Traversable
        {
            public record Component : Component<Component, b.Hex>
            {
                public required r.Multi<b.Player> ByPlayers { get; init; }
                public override ComponentIdentifier<Component> Identifier => _identifier;
                private static Identifier _identifier = new();
            }
            public record Identifier : ComponentIdentifier<Component>
            {
                public override string Identity => _identity;
                private static string _identity = "hex.traversable";
            }
        }
    }
}