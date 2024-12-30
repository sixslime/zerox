using System;
using Perfection;
#nullable enable
namespace FourZeroOne.Plugins.Axiom.Resolutions
{
    using r = Core.Resolutions;
    using ResObj = Resolution.IResolution;
    using ro = Core.Resolutions.Objects;
    using ax = GameObjects;
    using Resolution;
    using FourZeroOne.Proxy;
    using FourZeroOne.Core.Macros;
    using Core.Syntax;

    namespace GameObjects
    {
        namespace Unit
        {
            public record Data : ICompositionType
            {
                public readonly static StaticComponentIdentifier<Data, ro.Number> HP = new("axiom", "hp");
                public readonly static StaticComponentIdentifier<Data, Hex.Position> POSITION = new("axiom", "position");
                public readonly static StaticComponentIdentifier<Data, Player.Address> OWNER = new("axiom", "owner");
                public readonly static StaticComponentIdentifier<Data, r.Multi<NEffect>> EFFECTS = new("axiom", "effects");
            }
            public record Address : NoOp, IStateAddress<CompositionOf<Data>>
            {
                public required int ID { get; init; }
                public Address() { }
                public override string ToString()
                {
                    return $"@unit{ID}";
                }
            }
        }
        namespace Hex
        {
            public sealed record Data : ICompositionType
            {
                public readonly static StaticComponentIdentifier<Data, ro.Bool> CONTROL_POINT = new("axiom", "control_point");
                public readonly static StaticComponentIdentifier<Data, ro.Bool> OPEN = new("axiom", "open");
                public readonly static StaticComponentIdentifier<Data, ro.Bool> WALL = new("axiom", "wall");
                public readonly static StaticComponentIdentifier<Data, Player.Address> PLAYER_BASE = new("axiom", "player_base");
            }
            public sealed record Position : NoOp, IStateAddress<CompositionOf<Data>>
            {
                public required int R { get; init; }
                public required int U { get; init; }
                public required int D { get; init; }
                public Position() { }
                public Position Transform(Func<int, int, int> transformFunction, Position other)
                {
                    return new() { R = transformFunction(R, other.R), U = transformFunction(U, other.U), D = transformFunction(D, other.D) };
                }
                public override string ToString()
                {
                    return $"@hex{R}.{U}.{D}";
                }
            }
        }
        namespace Player
        {
            public sealed record Data : ICompositionType
            {
                
            }
            public sealed record Address : NoOp, IStateAddress<CompositionOf<Data>>
            {
                public required int ID { get; init; }
                public Address() { }
                public override string ToString()
                {
                    return $"@player{ID}";
                }
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
            public override IEnumerable<IInstruction> Instructions => [];
            protected NEffect(byte effectId)
            {
                EffectID = effectId;
            }
            public virtual bool Equals(NEffect? other)
            {
                return other is NEffect effect && effect.EffectID == EffectID;
            }
            public override int GetHashCode()
            {
                return EffectID;
            }
        }

    }
    namespace Structures
    {
        // this could be a composition.
        // then use a macro to resolve it into a multi
        public record HexArea : NoOp, IMulti<ax.Hex.Position>
        {
            public IEnumerable<ax.Hex.Position> Values => Offsets.Elements.Map(x => x.Transform((a, b) => a + b, Center));
            public int Count => Offsets.Count;
            public required ax.Hex.Position Center { get; init; }
            public required PList<ax.Hex.Position> Offsets { get; init; }
        }
    }
    namespace Action
    {
        public interface IAction<Self> : IDecomposableType<Self> where Self : IAction<Self>, new() { }
        public record Change<A, C> : IAction<Change<A, C>> where A : class, IStateAddress<ICompositionOf<C>>, ResObj where C : ICompositionType
        {
            public IProxy<Decompose<Change<A, C>>, ResObj> DecompositionProxy => MakeProxy.Statement<Decompose<Change<A, C>>, ResObj>(
                P =>
                    P.pSubEnvironment(RHint<ResObj>.Hint(), new()
                    {
                        Environment = P.pOriginalA().pAsVariable(out var thisObj),
                        Value = thisObj.tRef().tGetComponent(ADDRESS).tDataUpdate(RHint<ICompositionOf<C>>.Hint(),
                            subject => subject.tRef().tMerge(thisObj.tRef().tGetComponent(CHANGE))).pDirect(P)
                    }));
            public readonly static StaticComponentIdentifier<Change<A, C>, A> ADDRESS = new("axiom", "address");
            public readonly static StaticComponentIdentifier<Change<A, C>, ICompositionOf<r.MergeSpec<C>>> CHANGE = new("axiom", "change");
        }

    }
}