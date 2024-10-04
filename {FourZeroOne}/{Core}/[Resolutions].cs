
using Perfection;
using System.Collections.Generic;

#nullable enable
namespace FourZeroOne.Core.Resolutions
{
    using ResObj = Resolution.IResolution;
    using Token;
    using Resolution;

    namespace Objects
    {
        namespace Board
        {
            using Resolution.Board;
            using Objects;
            public sealed record Coordinates : NoOp
            {
                public required int R { get; init; }
                public required int U { get; init; }
                public required int D { get; init; }
                public int this[int i] => i switch
                {
                    0 => R,
                    1 => U,
                    2 => D,
                    _ => throw new System.IndexOutOfRangeException("Attempted to index Coordinates out of 0..2 range.")
                };
                public override string ToString() => $"({R}.{U}.{D})";
                public Coordinates Add(Coordinates b) => new()
                {
                    R = R + b.R,
                    U = U + b.U,
                    D = D + b.D,
                };
                public Coordinates ScalarManipulate(System.Func<int, int> scalarFunction) => new()
                {
                    R = scalarFunction(R),
                    U = scalarFunction(U),
                    D = scalarFunction(D)
                };
            }
            public sealed record CoordinateArea : NoOp, IMulti<Coordinates>
            {
                public IEnumerable<Coordinates> Values => Offsets.Map(x => x.Add(Center));
                public int Count => _offsets.Count;
                public required IEnumerable<Coordinates> Offsets { get => _offsets.Elements; init => _offsets = new() { Elements = value }; }
                public Updater<IEnumerable<Coordinates>> dOffsets { init => Offsets = value(Offsets); }
                public required Coordinates Center { get; init; }
                public Updater<Coordinates> dCenter { init => Center = value(Center); }
                public CoordinateArea()
                {
                    _offsets = new() { Elements = [] };
                }

                private PList<Coordinates> _offsets;
            }
            public sealed record Hex : StateObject<Hex>, IPositioned
            {
                public required Coordinates Position { get; init; }
                public Updater<Coordinates> dPosition { init => Position = value(Position); }
                
                public Hex(int id) : base(id) { }
                public override Hex GetAtState(State state)
                {
                    return state.Board.Hexes[UUID].Unwrap();
                }
                public override State SetAtState(State state)
                {
                    return state with
                    {
                        dBoard = Q => Q with
                        {
                            dHexes = Q => Q with
                            {
                                dElements = E => E.Also(this.Yield())
                            }
                        }
                    };
                }

                public override bool ResEqual(IResolution? other)
                {
                    return (other is Hex h && Position.ResEqual(h.Position));
                }
            }
            public sealed record Unit : StateObject<Unit>, IPositioned
            {
                public required Number HP { get; init; }
                public Updater<Number> dHP { init => HP = value(HP); }
                public required Coordinates Position { get; init; }
                public Updater<Coordinates> dPosition { init => Position = value(Position); }
                public required Player Owner { get; init; }
                public Updater<Player> dOwner { init => Owner = value(Owner); }
                public Unit(int id) : base(id) { }
                public override bool ResEqual(IResolution? other)
                {
                    return (other is Unit u && UUID == u.UUID);
                }

                public override Unit GetAtState(State state)
                {
                    return state.Board.Units[UUID].Unwrap();
                }

                public override State SetAtState(State state)
                {
                    return state with
                    {
                        dBoard = Q => Q with
                        {
                            dUnits = Q => Q with
                            {
                                dElements = E => E.Also(this.Yield())
                            }
                        }
                    };
                }

            }
            public sealed record Player : StateObject<Player>
            {
                public Player(int id) : base(id) { }

                public override Player GetAtState(State state)
                {
                    return state.Board.Players[UUID].Unwrap();
                }

                public override State SetAtState(State state)
                {
                    return state with
                    {
                        dBoard = Q => Q with
                        {
                            dPlayers = Q => Q with
                            {
                                dElements = E => E.Also(this.Yield())
                            }
                        }
                    };
                }

            }
        }
        public sealed record BoxedToken<R> : NoOp where R : class, ResObj
        {
            public required IToken<R> Token { get; init; }
            public override string ToString() => $"{Token}!";
        }
        public sealed record Number : NoOp
        {
            public required int Value { get; init; }
            public Updater<int> dValue { init => Value = value(Value); }
            public static implicit operator Number(int value) => new() { Value = value };
            public override string ToString() => $"{Value}";
        }
        public sealed record Bool : NoOp
        {
            public required bool IsTrue { get; init; }
            public Updater<bool> dIsTrue { init => IsTrue = value(IsTrue); }
            public static implicit operator Bool(bool value) => new() { IsTrue = value };
            public override string ToString() => $"{IsTrue}";
        }
    }
    namespace Actions
    {
        using Objects;

        namespace Board
        {
            using b = Objects.Board;

            namespace Unit
            {
                using static _.InternalUtil;
                public sealed record HPChange : Operation
                {
                    public required b.Unit Subject { get; init; }
                    public Updater<b.Unit> dSubject { init => Subject = value(Subject); }
                    public required Number SetTo { get; init; }
                    public Updater<Number> dSetTo { init => SetTo = value(SetTo); }
                    public HPChange() { }
                    protected override State UpdateState(State state) => ChangeUnit(state, Subject, x => x with { HP = SetTo });
                }
                public sealed record PositionChange : Operation
                {
                    public required b.Unit Subject { get; init; }
                    public Updater<b.Unit> dSubject { init => Subject = value(Subject); }
                    public required b.Coordinates SetTo { get; init; }
                    public Updater<b.Coordinates> dSetTo { init => SetTo = value(SetTo); }
                    public PositionChange() { }
                    protected override State UpdateState(State state) => ChangeUnit(state, Subject, x => x with { Position = SetTo });
                }
                public sealed record OwnerChange : Operation
                {
                    public required b.Unit Subject { get; init; }
                    public Updater<b.Unit> dSubject { init => Subject = value(Subject); }
                    public required b.Player SetTo { get; init; }
                    public Updater<b.Player> dSetTo { init => SetTo = value(SetTo); }
                    public OwnerChange() { }
                    protected override State UpdateState(State state) => ChangeUnit(state, Subject, x => x with { Owner = SetTo });
                }
            }

            namespace _
            {
                public static class InternalUtil
                {
                    public static State ChangeUnit(State state, b.Unit unit, Func<b.Unit, b.Unit> change)
                    {
                        return change(unit).SetAtState(state);
                    }
                }
            }
        }

        namespace Component
        {
            public sealed record Insert<H> : Operation where H : IHasComponents<H>
            {
                public required H ComponentHolder { get; init; }
                public required Multi<Resolution.Unsafe.IComponentFor<H>> Components { get; init; }

                protected override State UpdateState(State context)
                {
                    return ComponentHolder
                        .WithComponents(Components.Values)
                        .SetAtState(context);
                }
            }
            public sealed record Remove<H> : Operation where H : IHasComponents<H>
            {
                public required H ComponentHolder { get; init; }
                public required Multi<Resolution.Unsafe.IComponentIdentifier> Identifiers { get; init; }

                protected override State UpdateState(State context)
                {
                    return ComponentHolder
                        .WithoutComponents(Identifiers.Values)
                        .SetAtState(context);
                }
            }
        }
        
        public sealed record VariableAssign<R> : Operation where R : class, ResObj
        {
            public readonly VariableIdentifier<R> Identifier;
            public required IOption<R> Object { get; init; }
            public Updater<IOption<R>> dObject { init => Object = value(Object); }
            public VariableAssign(VariableIdentifier<R> identifier)
            {
                Identifier = identifier;
            }
            protected override State UpdateState(State state) => state with
            {
                dVariables = Q => Q with
                {
                    dElements = Q => Q.Also(((Token.Unsafe.VariableIdentifier)Identifier, (IOption<ResObj>)Object).Yield())
                }
            };
            public override string ToString() => $"{Identifier}<-{Object}";
        }
        public sealed record RuleAdd : Operation
        {
            public required Rule.IRule Rule { get; init; }
            public Updater<Rule.IRule> dRule { init => Rule = value(Rule); }

            protected override State UpdateState(State state) => state with
            {
                dRules = Q => Q with { dElements = Q => Q.Also(Rule.Yield()) }
            };
        }
    }
    public sealed record Multi<R> : Operation, IMulti<R> where R : class, ResObj
    {
        public int Count => _list.Count;
        public required IEnumerable<R> Values { get => _list.Elements; init => _list = new() { Elements = value }; }
        public Updater<IEnumerable<R>> dValues { init => Values = value(Values); }
        public override bool ResEqual(ResObj? other)
        {
            if (other is not IMulti<R> othermulti) return false;
            foreach (var (a, b) in Values.ZipLong(othermulti.Values)) if (a is null || (a is not null && !a.ResEqual(b))) return false;
            return true;
        }
        protected override State UpdateState(State state)
        {
            return Values.AccumulateInto(state, (p, x) => p.WithResolution(x));
        }

        private readonly PList<R> _list;
        public override string ToString()
        {
            List<R> argList = [.. _list.Elements];
            return $"[{argList[0]}{argList[1..].AccumulateInto("", (msg, v) => $"{msg}, {v}")}]";
        }
    }
}