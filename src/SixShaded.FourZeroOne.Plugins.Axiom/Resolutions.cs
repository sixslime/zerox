namespace SixShaded.FourZeroOne.Plugins.Axiom
{
    using Core.Resolutions;
    using Core.Syntax;
    using Resolution;
    using Resolution.Defined;

    // GOAL: create an attack ability
    // 'current turn' static player pointer?
    namespace State
    {
        public record ActingPlayer : IMemoryAddress<GameObjects.Player.Address>
        {
            public static readonly ActingPlayer PTR = new();
            private ActingPlayer() { }
            public override string ToString() => "ACTING_PLAYER";
        }

        public record TurnCount : IMemoryAddress<Number>
        {
            public static readonly TurnCount PTR = new();
            private TurnCount() { }
            public override string ToString() => "TURN_COUNT";
        }
    }

    namespace GameObjects
    {
        namespace Unit
        {
            public record Data : ICompositionType
            {
                public static readonly StaticComponentIdentifier<Data, Number> HP = new("axiom", "hp");

                public static readonly StaticComponentIdentifier<Data, Hex.Position>
                    POSITION = new("axiom", "position");

                public static readonly StaticComponentIdentifier<Data, Player.Address> OWNER = new("axiom", "owner");

                public static readonly StaticComponentIdentifier<Data, Multi<NEffect>>
                    EFFECTS = new("axiom", "effects");
            }

            public record Address : NoOp, IMemoryObject<CompositionOf<Data>>
            {
                public required int ID { get; init; }

                public override string ToString() => $"@unit{ID}";
            }
        }

        namespace Hex
        {
            public sealed record Data : ICompositionType
            {
                public static readonly StaticComponentIdentifier<Data, Bool> CONTROL_POINT =
                    new("axiom", "control_point");

                public static readonly StaticComponentIdentifier<Data, Bool> OPEN = new("axiom", "open");
                public static readonly StaticComponentIdentifier<Data, Bool> WALL = new("axiom", "wall");

                public static readonly StaticComponentIdentifier<Data, Player.Address> PLAYER_BASE =
                    new("axiom", "player_base");
            }

            public sealed record Position : NoOp, IMemoryObject<CompositionOf<Data>>
            {
                public required int R { get; init; }
                public required int U { get; init; }
                public required int D { get; init; }

                public Position Transform(Func<int, int, int> transformFunction, Position other) => new()
                {
                    R = transformFunction(R, other.R),
                    U = transformFunction(U, other.U),
                    D = transformFunction(D, other.D),
                };

                public override string ToString() => $"@hex{R}.{U}.{D}";
            }
        }

        namespace Player
        {
            public sealed record Data : ICompositionType
            { }

            public sealed record Address : NoOp, IMemoryAddress<CompositionOf<Data>>
            {
                public required int ID { get; init; }

                public override string ToString() => $"@player{ID}";
            }
        }

        // weirdchamp implementation of enums/flags but i think it works probably.
        // make a Multi<NEffect>
        public record NEffect : NoOp
        {
            public static readonly NEffect SLOW = new(1);
            public static readonly NEffect SILENCE = new(2);
            public static readonly NEffect ROOT = new(3);
            public static readonly NEffect STUN = new(4);

            public readonly byte EffectID;

            protected NEffect(byte effectId)
            {
                EffectID = effectId;
            }

            public override IEnumerable<IInstruction> Instructions => [];

            public virtual bool Equals(NEffect? other) => other is not null && other.EffectID == EffectID;

            public override int GetHashCode() => EffectID;
        }
    }

    namespace Structures
    {
        // this could be a composition.
        // then use a macro to resolve it into a multi
        /*
        public record HexArea : NoOp, IMulti<ax.Hex.Position>
        {
            public IEnumerable<ax.Hex.Position> ValueSequence => Offsets.Elements.Map(x => x.Transform((a, b) => a + b, Center));
            public int Count => Offsets.Count;
            public required ax.Hex.Position Center { get; init; }
            public required PList<ax.Hex.Position> Offsets { get; init; }
        }
        */
    }

    namespace Action
    {
        public interface IAction<Self> : IDecomposableType<Self, Res> where Self : IAction<Self>, new()
        { }

        public record Change<C> : IAction<Change<C>> where C : ICompositionType
        {
            public static readonly StaticComponentIdentifier<Change<C>, IMemoryObject<ICompositionOf<C>>> ADDRESS =
                new("axiom", "address");

            public static readonly StaticComponentIdentifier<Change<C>, ICompositionOf<MergeSpec<C>>> CHANGE =
                new("axiom", "change");

            public MetaFunction<ICompositionOf<Change<C>>, Res> DecompositionFunction =>
                Core.tMetaFunction<ICompositionOf<Change<C>>, Res>(
                        thisObj =>
                            thisObj.tRef()
                                .tGetComponent(ADDRESS)
                                .tDataUpdate(
                                    subject =>
                                        subject.tRef()
                                            .tMerge(thisObj.tRef().tGetComponent(CHANGE))))
                    .Resolution;
        }
    }
}