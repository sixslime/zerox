namespace SixShaded.FourZeroOne.Plugins.Axiom
{
    using Core.Roggis;
    using Core.Syntax;
    using Roggi;
    using Roggi.Defined;

    // GOAL: create an attack ability
    // 'current turn' static player pointer?
    namespace State
    {
        public record ActingPlayer : IMemoryAddress<GameObjects.Player.Address>
        {
            public static readonly ActingPlayer PTR = new();

            private ActingPlayer()
            { }

            public override string ToString() => "ACTING_PLAYER";
        }

        public record TurnCount : IMemoryAddress<Number>
        {
            public static readonly TurnCount PTR = new();

            private TurnCount()
            { }

            public override string ToString() => "TURN_COUNT";
        }
    }

    namespace GameObjects
    {
        namespace Unit
        {
            public class Data : Roveggitu
            {
                public static readonly Rovu<Data, Number> HP = new(Axoi.Du, "hp");

                public static readonly Rovu<Data, Hex.Position>
                    POSITION = new(Axoi.Du, "position");

                public static readonly Rovu<Data, Player.Address> OWNER = new(Axoi.Du, "owner");

                public static readonly Rovu<Data, Multi<NEffect>>
                    EFFECTS = new(Axoi.Du, "effects");
            }

            public record Address : NoOp, IMemoryObject<Roveggi<Data>>
            {
                public required int ID { get; init; }

                public override string ToString() => $"@unit{ID}";
            }
        }

        namespace Hex
        {
            public sealed class Data : Roveggitu
            {
                public static readonly Rovu<Data, Bool> CONTROL_POINT =
                    new(Axoi.Du, "control_point");

                public static readonly Rovu<Data, Bool> OPEN = new(Axoi.Du, "open");
                public static readonly Rovu<Data, Bool> WALL = new(Axoi.Du, "wall");

                public static readonly Rovu<Data, Player.Address> PLAYER_BASE =
                    new(Axoi.Du, "player_base");
            }

            public sealed record Position : NoOp, IMemoryObject<Roveggi<Data>>
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
            public sealed class Data : Roveggitu
            { }

            public sealed record Address : NoOp, IMemoryAddress<Roveggi<Data>>
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
        using FourZeroOne.Core.Roveggitus;

        public interface IAction<Self> : IDecomposableRoveggitu<Self, Res> where Self : IAction<Self>, new()
        { }

        public class Change<C> : IAction<Change<C>> where C : Roveggitu
        {
            public static readonly Rovu<Change<C>, IMemoryObject<IRoveggi<C>>> ADDRESS =
                new(Axoi.Du, "address");

            public static readonly Rovu<Change<C>, IRoveggi<MergeSpec<C>>> CHANGE =
                new(Axoi.Du, "change");

            public MetaFunction<IRoveggi<Change<C>>, Res> DecomposeFunction =>
                Core.tMetaFunction<IRoveggi<Change<C>>, Res>(
                    thisObj =>
                        thisObj.tRef()
                            .tGetComponent(ADDRESS)
                            .tMemoryUpdate(
                            subject =>
                                subject.tRef()
                                    .tMerge(thisObj.tRef().tGetComponent(CHANGE))))
                    .Roggi;
        }
    }
}