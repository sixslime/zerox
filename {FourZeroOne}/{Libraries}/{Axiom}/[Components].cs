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
    namespace Unit
    {
        using Core.Resolutions.Objects.Board;
        namespace Effects
        {
            namespace Slow
            {
                public record Component : Component<Component, Unit>
                {
                    public override ComponentIdentifier<Component> Identifier => _identifier;
                    private static Identifier _identifier = new();
                }
                public record Identifier : ComponentIdentifier<Component>
                {
                    public override string Identity => _identity;
                    private static string _identity = "slow";
                }
            }
        }
    }
}