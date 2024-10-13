
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
                
                public Hex() : base() { }
                public override Hex GetAtState(State state)
                {
                    return state.Board.Hexes[this].Unwrap();
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
                public override State RemoveAtState(State state)
                {
                    return state with
                    {
                        dBoard = Q => Q with
                        {
                            dHexes = D => D with
                            {
                                dElements = E => E.ExceptBy(D.IndexGenerator(this).Yield(), D.IndexGenerator)
                            }
                        }
                    };
                }

                public override bool ResEqual(IResolution? other)
                {
                    return (other is Hex h && h.Position.ResEqual(Position));
                }
            }
            public sealed record Unit : StateObject<Unit>, IPositioned
            {
                public readonly int UUID;
                // We could totally make HP and Owner (and even position) into components.
                // Why should we? Why shouldn't we.
                public required Number HP { get; init; }
                public Updater<Number> dHP { init => HP = value(HP); }
                public required Coordinates Position { get; init; }
                public Updater<Coordinates> dPosition { init => Position = value(Position); }
                public required Player Owner { get; init; }
                public Updater<Player> dOwner { init => Owner = value(Owner); }
                public Unit(int id) : base()
                {
                    UUID = id;
                }
                public override bool ResEqual(IResolution? other)
                {
                    return (other is Unit u && UUID == u.UUID);
                }

                public override Unit GetAtState(State state)
                {
                    return state.Board.Units[this].Unwrap();
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
                public override State RemoveAtState(State state)
                {
                    return state with
                    {
                        dBoard = Q => Q with
                        {
                            dUnits = D => D with
                            {
                                //REFACTOR: A new IndexedSet or something so this doesnt have to happen.
                                // Also PIndexedSet<I, T> should map to a PSet<T>,
                                dElements = E => E.ExceptBy(D.IndexGenerator(this).Yield(), D.IndexGenerator)
                            }
                        }
                    };
                }

            }
            public sealed record Player : StateObject<Player>
            {
                public readonly int UUID;
                public Player(int id) : base()
                {
                    UUID = id;
                }

                public override Player GetAtState(State state)
                {
                    return state.Board.Players[this].Unwrap();
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
                public override State RemoveAtState(State state)
                {
                    return state with
                    {
                        dBoard = Q => Q with
                        {
                            dPlayers = D => D with
                            {
                                dElements = E => E.ExceptBy(D.IndexGenerator(this).Yield(), D.IndexGenerator)
                            }
                        }
                    };
                }
            }
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

        // This is stupid, this is stupid, this is stupid.
        // DONT ADD TUPLES CHALLENGE (VERY HARD)
        public sealed record Range : NoOp, IMulti<Number>
        {
            
            public required Number Start { get; init; }
            public required Number End { get; init; }

            public IEnumerable<Number> Values => (Start.Value <= End.Value)
                ? Start.Sequence(x => x with { dValue = Q => Q + 1 }).TakeWhile(x => x.Value <= End.Value)
                : [];
            public int Count => (Start.Value <= End.Value) ? (End.Value - Start.Value) + 1 : 0;
            public override string ToString() => $"{Start}..{End}";
        }

    }
    namespace Instructions
    {
        using Objects;

        namespace Board
        {
            using b = Objects.Board;

            namespace Unit
            {
                using static _.InternalUtil;
                public sealed record HPChange : Instruction
                {
                    public required b.Unit Subject { get; init; }
                    public Updater<b.Unit> dSubject { init => Subject = value(Subject); }
                    public required Number SetTo { get; init; }
                    public Updater<Number> dSetTo { init => SetTo = value(SetTo); }
                    public HPChange() { }
                    public override State ChangeState(State state) => ChangeUnit(state, Subject, x => x with { HP = SetTo });
                }
                public sealed record PositionChange : Instruction
                {
                    public required b.Unit Subject { get; init; }
                    public Updater<b.Unit> dSubject { init => Subject = value(Subject); }
                    public required b.Coordinates SetTo { get; init; }
                    public Updater<b.Coordinates> dSetTo { init => SetTo = value(SetTo); }
                    public PositionChange() { }
                    public override State ChangeState(State state) => ChangeUnit(state, Subject, x => x with { Position = SetTo });
                }
                public sealed record OwnerChange : Instruction
                {
                    public required b.Unit Subject { get; init; }
                    public Updater<b.Unit> dSubject { init => Subject = value(Subject); }
                    public required b.Player SetTo { get; init; }
                    public Updater<b.Player> dSetTo { init => SetTo = value(SetTo); }
                    public OwnerChange() { }
                    public override State ChangeState(State state) => ChangeUnit(state, Subject, x => x with { Owner = SetTo });
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
            public sealed record Insert<H> : Instruction where H : IHasComponents<H>
            {
                public required H ComponentHolder { get; init; }
                public required Multi<Resolution.Unsafe.IComponentFor<H>> Components { get; init; }

                public override State ChangeState(State context)
                {
                    return ComponentHolder
                        .WithComponents(Components.Values)
                        .SetAtState(context);
                }
            }
            public sealed record Remove<H> : Instruction where H : IHasComponents<H>
            {
                public required H ComponentHolder { get; init; }
                public required Multi<Resolution.Unsafe.IComponentIdentifier> Identifiers { get; init; }

                public override State ChangeState(State context)
                {
                    return ComponentHolder
                        .WithoutComponents(Identifiers.Values)
                        .SetAtState(context);
                }
            }
        }
        
        public sealed record Declare : Instruction
        {
            public required Resolution.Unsafe.IStateTracked Subject { get; init; }
            public override State ChangeState(State context)
            {
                return Subject.SetAtState(context);
            }
        }
        public sealed record Undeclare : Instruction
        {
            public required Resolution.Unsafe.IStateTracked Subject { get; init; }
            public override State ChangeState(State context)
            {
                return Subject.RemoveAtState(context);
            }
        }
        public sealed record VariableAssign<R> : Instruction where R : class, ResObj
        {
            public readonly VariableIdentifier<R> Identifier;
            public required IOption<R> Object { get; init; }
            public Updater<IOption<R>> dObject { init => Object = value(Object); }
            public VariableAssign(VariableIdentifier<R> identifier)
            {
                Identifier = identifier;
            }
            public override State ChangeState(State state) => state with
            {
                dVariables = Q => Q with
                {
                    dElements = Q => Q.Also(((Token.Unsafe.VariableIdentifier)Identifier, (IOption<ResObj>)Object).Yield())
                }
            };
            public override string ToString() => $"{Identifier}<-{Object}";
        }
        public sealed record RuleAdd : Instruction
        {
            public required Rule.IRule Rule { get; init; }
            public Updater<Rule.IRule> dRule { init => Rule = value(Rule); }

            public override State ChangeState(State state) => state with
            {
                dRules = Q => Q with { dElements = Q => Q.Also(Rule.Yield()) }
            };
        }
    }
    namespace Boxed
    {
        public sealed record MetaFunction<R> : NoOp where R : class, ResObj
        {
            public required VariableIdentifier <MetaFunction<R>> SelfIdentifier { get; init; }
            public required IToken<R> Token { get; init; }
            public override string ToString() => $"{SelfIdentifier}()-> {{{Token}}}";
        }
        public sealed record MetaFunction<RArg1, ROut> : NoOp
            where RArg1 : class, ResObj
            where ROut : class, ResObj
        {
            public required VariableIdentifier <MetaFunction<RArg1, ROut>> SelfIdentifier { get; init; }
            public required VariableIdentifier<RArg1> IdentifierA { get; init; }
            public required IToken<ROut> Token { get; init; }
            public override string ToString() => $"{SelfIdentifier}({IdentifierA})-> {{{Token}}}";
        }
        public sealed record MetaFunction<RArg1, RArg2, ROut> : NoOp
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where ROut : class, ResObj
        {
            public required VariableIdentifier <MetaFunction<RArg1, RArg2, ROut>> SelfIdentifier { get; init; }
            public required VariableIdentifier<RArg1> IdentifierA { get; init; }
            public required VariableIdentifier<RArg2> IdentifierB { get; init; }
            public required IToken<ROut> Token { get; init; }
            public override string ToString() => $"({IdentifierA}, {IdentifierB})-> {SelfIdentifier}{{{Token}}}";
        }
        public sealed record MetaFunction<RArg1, RArg2, RArg3, ROut> : NoOp
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where RArg3 : class, ResObj
            where ROut : class, ResObj
        {
            public required VariableIdentifier <MetaFunction<RArg1, RArg2, RArg3, ROut>> SelfIdentifier { get; init; }
            public required VariableIdentifier<RArg1> IdentifierA { get; init; }
            public required VariableIdentifier<RArg2> IdentifierB { get; init; }
            public required VariableIdentifier<RArg3> IdentifierC { get; init; }
            public required IToken<ROut> Token { get; init; }
            public override string ToString() => $"({IdentifierA}, {IdentifierB}, {IdentifierC})-> {SelfIdentifier}{{{Token}}}";
        }

        public sealed record MetaArgs<R1> : NoOp
            where R1 : class, ResObj
        {
            public required IOption<R1> Arg1 { get; init; }
            public override string ToString() => $"<{Arg1}>";
        }
        public sealed record MetaArgs<R1, R2> : NoOp
            where R1 : class, ResObj
            where R2 : class, ResObj
        {
            public required IOption<R1> Arg1 { get; init; }
            public required IOption<R2> Arg2 { get; init; }
            public override string ToString() => $"<{Arg1},{Arg2}>";
        }
        public sealed record MetaArgs<R1, R2, R3> : NoOp
            where R1 : class, ResObj
            where R2 : class, ResObj
            where R3 : class, ResObj
        {
            public required IOption<R1> Arg1 { get; init; }
            public required IOption<R2> Arg2 { get; init; }
            public required IOption<R3> Arg3 { get; init; }
            public override string ToString() => $"<{Arg1},{Arg2},{Arg3}>";
        }

    }
    public sealed record Multi<R> : Composition, IMulti<R> where R : class, ResObj
    {
        public override IEnumerable<IInstruction> Instructions => Values.Map(x => x.Instructions).Flatten();
        public int Count => _list.Count;
        public required IEnumerable<R> Values { get => _list.Elements; init => _list = new() { Elements = value }; }
        public Updater<IEnumerable<R>> dValues { init => Values = value(Values); }
        public override bool ResEqual(ResObj? other)
        {
            if (other is not IMulti<R> othermulti) return false;
            foreach (var (a, b) in Values.ZipLong(othermulti.Values)) if (a is null || (a is not null && !a.ResEqual(b))) return false;
            return true;
        }

        private readonly PList<R> _list;
        public override string ToString()
        {
            List<R> argList = [.. _list.Elements];
            return $"[{argList[0]}{argList[1..].AccumulateInto("", (msg, v) => $"{msg}, {v}")}]";
        }
    }
}