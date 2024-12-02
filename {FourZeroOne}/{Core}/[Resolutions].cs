
using Perfection;
using System.Collections.Generic;

#nullable enable
namespace FourZeroOne.Core.Resolutions
{
    using ResObj = Resolution.IResolution;
    using Token;
    using Resolution;
    using Resolution.Unsafe;

    namespace Objects
    {
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
        public sealed record NumRange : NoOp, IMulti<Number>
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
        public sealed record Assign<D> : Instruction where D : class, ResObj
        {
            public required IStateAddress<D> Address { get; init; }
            public required D Subject { get; init; }
            public override IState ChangeState(IState previousState)
            {
                return previousState.WithObjects([(Address, Subject)]);
            }
            public override string ToString() => $"{Address}<-{Subject}";
        }

        public record Merge<H> : ICompositionType where H : ICompositionType, new()
        {
            public ICompositionType.ResolutionFunction ResolvesTo => components =>
            {
                return components[SUBJECT].Check(out var sObj) && sObj is ICompositionOf<H> subject
                    ? subject
                        .WithComponentsUnsafe(
                            components.Elements
                            .Where(x => x.key is _Private.IMerger)
                            .Map(x => (((_Private.IMerger)x.key).ForComponent, x.val)))
                        .AsSome()
                    : new None<ResObj>();
            };
            public readonly static StaticComponentIdentifier<Merge<H>, ICompositionOf<H>> SUBJECT = new("CORE", "subject");
            public static _Private.MergeComponentIdentifier<H, R> MERGE<R>(IComponentIdentifier<H, R> component) where R : class, ResObj => new(component);
        }
        namespace _Private
        {
            public interface IMerger
            {
                public IComponentIdentifier ForComponent { get; }
            }
            public record MergeComponentIdentifier<H, R> : IComponentIdentifier<Merge<H>, R>, IMerger where H : ICompositionType, new() where R : class, ResObj
            {
                public IComponentIdentifier ForComponent { get; private init; }
                public string Source => "CORE";
                public string Identity => $"merge-{ForComponent.Identity}";
                public MergeComponentIdentifier(IComponentIdentifier<H, R> component)
                {
                    ForComponent = component;
                }
            }
        }

        public sealed record Redact : Instruction
        {
            public required IStateAddress Address { get; init; }
            public override IState ChangeState(IState context)
            {
                return context.WithClearedAddresses([Address]);
            }
        }
        public sealed record RuleAdd : Instruction
        {
            public required Rule.IRule Rule { get; init; }
            public override IState ChangeState(IState state)
            {
                return state.WithRules([Rule]);
            }
        }
    }
    namespace Boxed
    {
        public sealed record MetaFunction<R> : NoOp where R : class, ResObj
        {
            public required DynamicAddress<MetaFunction<R>> SelfIdentifier { get; init; }
            public required IToken<R> Token { get; init; }
            public override string ToString() => $"()-> {SelfIdentifier}{{{Token}}}";
        }
        public sealed record MetaFunction<RArg1, ROut> : NoOp
            where RArg1 : class, ResObj
            where ROut : class, ResObj
        {
            public required DynamicAddress <MetaFunction<RArg1, ROut>> SelfIdentifier { get; init; }
            public required DynamicAddress<RArg1> IdentifierA { get; init; }
            public required IToken<ROut> Token { get; init; }
            public override string ToString() => $"({IdentifierA})-> {SelfIdentifier}{{{Token}}}";
        }
        public sealed record MetaFunction<RArg1, RArg2, ROut> : NoOp
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where ROut : class, ResObj
        {
            public required DynamicAddress <MetaFunction<RArg1, RArg2, ROut>> SelfIdentifier { get; init; }
            public required DynamicAddress<RArg1> IdentifierA { get; init; }
            public required DynamicAddress<RArg2> IdentifierB { get; init; }
            public required IToken<ROut> Token { get; init; }
            public override string ToString() => $"({IdentifierA}, {IdentifierB})-> {SelfIdentifier}{{{Token}}}";
        }
        public sealed record MetaFunction<RArg1, RArg2, RArg3, ROut> : NoOp
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where RArg3 : class, ResObj
            where ROut : class, ResObj
        {
            public required DynamicAddress <MetaFunction<RArg1, RArg2, RArg3, ROut>> SelfIdentifier { get; init; }
            public required DynamicAddress<RArg1> IdentifierA { get; init; }
            public required DynamicAddress<RArg2> IdentifierB { get; init; }
            public required DynamicAddress<RArg3> IdentifierC { get; init; }
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

    public sealed record Multi<R> : Construct, IMulti<R> where R : class, ResObj
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