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
        namespace Unit
        {
            public sealed record Data : Composition<Data>
            {
                public override IEnumerable<IInstruction> Instructions => [];
            }
            public sealed record Identifier : NoOp, IStateAddress<Data>
            {
                public required int ID { get; init; }
                public Identifier() { }
            }
            public static class Component
            {
                public readonly static StaticComponentIdentifier<Data, ro.Number> HP = new("axiom", "unit.hp");
                public readonly static StaticComponentIdentifier<Data, Hex.Position> POSITION = new("axiom", "unit.position");
                public readonly static StaticComponentIdentifier<Data, Player.Identifier> OWNER = new("axiom", "unit.owner");
            }
        }
        namespace Hex
        {
            public sealed record Data : Composition<Data>
            {
                public override IEnumerable<IInstruction> Instructions => [];
            }
            public sealed record Position : NoOp, IStateAddress<Data>
            {
                public required int R { get; init; }
                public required int U { get; init; }
                public required int D { get; init; }
                public Position() { }
            }
            public static class Component
            {

            }
        }
        namespace Player
        {
            public sealed record Data : Composition<Data>
            {
                public override IEnumerable<IInstruction> Instructions => [];
            }
            public sealed record Identifier : NoOp, IStateAddress<Data>
            {
                public required int ID { get; init; }
                public Identifier() { }
            }
            public static class Component
            {

            }
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