using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourZeroOne.Libraries.Axiom.Resolutions
{
    using r = Core.Resolutions;
    using ro = Core.Resolutions.Objects;
    using Resolution;

    namespace Objects
    {
        public sealed record Unit : NoOp
        {

        }
    }
    /*
    namespace GameActions
    {
        namespace Move
        {
            public sealed record Numerical : Construct
            {
                public override IEnumerable<IInstruction> Instructions => [new PositionChange() { Subject = Unit, SetTo = Destination }];
                public required b.Unit Unit { get; init; }
                public required ro.Number Distance { get; init; }
                public required b.Coordinates Start { get; init; }
                public required b.Coordinates Destination { get; init; }
            }
        }
        // DEV - do we make the lang purely functional and make declare() the only real instruction where functions just modify the object and declare updates the state with the object?
        // if this is done, then, like coordinates map to hexes, perhaps we should have some sort of unit ID object (that is a resolution itself) map to units.
    }
    */
}