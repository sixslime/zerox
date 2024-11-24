using System;
using Perfection;
#nullable enable
namespace FourZeroOne.Libraries.Axiom.Resolutions
{
    using r = Core.Resolutions;
    using ro = Core.Resolutions.Objects;
    using ax = GameObjects;
    using Resolution;

    namespace GameObjects
    {
        namespace Unit
        {
            public record Data : Composition<Data>
            {
                public override IEnumerable<IInstruction> Instructions => [];
            }
            public record Identifier : NoOp, IStateAddress<Data>
            {
                public required int ID { get; init; }
                public Identifier() { }
            }
            public static class Component
            {
                public readonly static StaticComponentIdentifier<Data, ro.Number> HP = new("axiom", "hp");
                public readonly static StaticComponentIdentifier<Data, Hex.Position> POSITION = new("axiom", "position");
                public readonly static StaticComponentIdentifier<Data, Player.Identifier> OWNER = new("axiom", "owner");
                public readonly static StaticComponentIdentifier<Data, r.Multi<NEffect>> EFFECTS = new("axiom", "effects");
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
                public Position Transform(Func<int, int, int> transformFunction, Position other)
                {
                    return new() { R = transformFunction(R, other.R), U = transformFunction(U, other.U), D = transformFunction(D, other.D) };
                }
            }
            public static class Component
            {
                public readonly static StaticComponentIdentifier<Data, ro.Bool> CONTROL_POINT = new("axiom", "control_point");
                public readonly static StaticComponentIdentifier<Data, ro.Bool> OPEN = new("axiom", "open");
                public readonly static StaticComponentIdentifier<Data, ro.Bool> WALL = new("axiom", "wall");
                public readonly static StaticComponentIdentifier<Data, Player.Identifier> PLAYER_BASE = new("axiom", "player_base");
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

        // weirdchamp implementation of enums/flags but i think it works probably.
        // make a Multi<NEffect>
        public record NEffect : Composition<NEffect>
        {
            public static readonly NEffect SLOW = new(1);
            public static readonly NEffect SILENCE = new(2);
            public static readonly NEffect ROOT = new(3);
            public static readonly NEffect STUN = new(4);

            public readonly byte EffectID;
            public override IEnumerable<IInstruction> Instructions => [];
            protected NEffect(byte effectId)
            {
                EffectID = effectId;
            }
            public override bool ResEqual(IResolution? other)
            {
                return other is NEffect effect && effect.EffectID == EffectID;
            }
        }

    }
    namespace Structures
    {
        public record HexArea : NoOp, IMulti<ax.Hex.Position>
        {
            public IEnumerable<ax.Hex.Position> Values => Offsets.Elements.Map(x => x.Transform((a, b) => a + b, Center));
            public int Count => Offsets.Count;
            public required ax.Hex.Position Center { get; init; }
            public required PList<ax.Hex.Position> Offsets { get; init; }
        }
    }

    namespace Actions
    {
        namespace Unit
        {
            namespace Move
            {
                namespace Set
                {
                    public record Data : Composition<Data>
                    {
                        public override IEnumerable<IInstruction> Instructions => GetComponent(Component.MOVES).RemapAs(x => x.Values.Map(y => y.Instructions).Flatten()).Or([]);
                    }
                    public static class Component
                    {
                        public readonly static StaticComponentIdentifier<Data, r.Multi<r.Instructions.Merge.Data<ax.Unit.Identifier>>> MOVES = new("axiom", "moves");
                    }
                }
                public record Data : Composition<Data>
                {
                    public override IEnumerable<IInstruction> Instructions => [];
                }
                public static class Component
                {
                    public readonly static StaticComponentIdentifier<Data, ro.Number> DISTANCE = new("axiom", "distance");
                }
            }
        }
    }
    /*f
    namespace GameActions
    {
        namespace MoveSet
        {
            public record Data : Composition<Data>
            {
                public override IEnumerable<IInstruction> Instructions => Components[Component.MOVES].RemapAs(x => x.Instructions).Or([]);
            }
            public static class Component
            {
                public readonly static StaticComponentIdentifier<Data, r.Multi<Structures.Move.Data>> MOVES = new("axiom", "moves");
            }
        }
    }
    // yea we should have a MoveSet gameaction composed of individual moves

    // DeclareAction<R> is the hook for rules to change specific actions, while allowing additional actions, aswell as reading a single action.
    // make a rule for a specific DeclareAction<R>.
    // DeclareAction<R> is functionally equivalent to yield.
    // Multi<MoveAction> Move(...) => Multi<MoveAction> Declare<MoveAction>(MoveAction action) { yield action };
    
    // example rule: DeclareAction<MoveAction>(A) => Union(<other action>, DeclareAction<MoveAction>(A)).
    // the whole point is to always keep the declare action.
    
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