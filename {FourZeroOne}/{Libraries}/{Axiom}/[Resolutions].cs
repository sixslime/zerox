using System;
using Perfection;
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
                public readonly static StaticComponentIdentifier<Data, ro.Number> HP = new("axiom", "hp");
                public readonly static StaticComponentIdentifier<Data, Hex.Position> POSITION = new("axiom", "position");
                public readonly static StaticComponentIdentifier<Data, Player.Identifier> OWNER = new("axiom", "owner");
                public readonly static StaticComponentIdentifier<Data, r.Multi<Effect>> EFFECTS = new("axiom", "effects");
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
                public Position Add(Position other)
                {
                    return new() { R = R + other.R, U = U + other.U, D = D + other.D };
                }
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

        // weirdchamp implementation of enums/flags but i think it works probably.
        // make a Multi<Effect>
        public abstract record Effect : NoOp
        {
            public Effect(byte effectId)
            {
                _effectId = effectId;
            }
            public override bool ResEqual(IResolution? other)
            {
                return other is Effect effect && effect._effectId == _effectId;
            }
            private readonly byte _effectId;
        }
        namespace Effects
        {
            public sealed record Slow : Effect { public Slow() : base(1) { } }
            public sealed record Root : Effect { public Root() : base(2) { } }
            public sealed record Stun : Effect { public Stun() : base(3) { } }
            public sealed record Silence : Effect { public Silence() : base(4) { } }
        }
    }
    namespace Structures
    {
        public sealed record HexArea : NoOp, IMulti<ax.Hex.Position>
        {
            public IEnumerable<ax.Hex.Position> Values => Offsets.Elements.Map(x => x.Add(Center));
            public int Count => Offsets.Count;
            public required ax.Hex.Position Center { get; init; }
            public required PList<ax.Hex.Position> Offsets { get; init; }
        }
    }

    namespace GameActions
    {
        namespace Move
        {
            public sealed record Data : Composition<Data>
            {
                public override IEnumerable<IInstruction> Instructions => throw new NotImplementedException();

            }
            public static class Component
            {
                public readonly static StaticComponentIdentifier<Data, ax.Hex.Position> START = new("axiom", "start");
                public readonly static StaticComponentIdentifier<Data, ax.Hex.Position> DESTINATION = new("axiom", "destination");

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