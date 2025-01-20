
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
             
            public IHasElements<Number> Container => new PSequence<Number>().WithEntries(
                (Start.Value <= End.Value)
                    ? Start.Sequence(x => x with { dValue = Q => Q + 1 }).TakeWhile(x => x.Value <= End.Value)
                    : []);
            public int Count => (Start.Value <= End.Value) ? (End.Value - Start.Value) + 1 : 0;
            public override string ToString() => $"{Start}..{End}";
        }

    }
    namespace Instructions
    {
        using FourZeroOne.Core.Macros;
        using FourZeroOne.Proxy;
        using FourZeroOne.Core.Syntax;
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
            public override string ToString()
            {
                return $"<?>+{Rule}";
            }
        }
    }
    namespace Boxed
    {
        public sealed record MetaFunction<R> : NoOp where R : class, ResObj
        {
            public required DynamicAddress<MetaFunction<R>> SelfIdentifier { get; init; }
            public required IToken<R> Token { get; init; }
            public override string ToString() => $"{SelfIdentifier}(){{{Token}}}";
        }
        public sealed record MetaFunction<RArg1, ROut> : NoOp
            where RArg1 : class, ResObj
            where ROut : class, ResObj
        {
            public required DynamicAddress <MetaFunction<RArg1, ROut>> SelfIdentifier { get; init; }
            public required DynamicAddress<RArg1> IdentifierA { get; init; }
            public required IToken<ROut> Token { get; init; }
            public override string ToString() => $"{SelfIdentifier}({IdentifierA})::{{{Token}}}";
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
            public override string ToString() => $"{SelfIdentifier}({IdentifierA}, {IdentifierB})::{{{Token}}}";
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
            public override string ToString() => $"{SelfIdentifier}({IdentifierA}, {IdentifierB}, {IdentifierC})::{{{Token}}}";
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
    public record MergeSpec<H> : ICompositionType where H : ICompositionType
    {

        public static _Private.MergeComponentIdentifier<H, R> MERGE<R>(IComponentIdentifier<H, R> component) where R : class, ResObj => new(component);

    }
    namespace _Private
    {
        // ??
        public interface IMergeIdentifier
        {
            public IComponentIdentifier ForComponentUnsafe { get; }
        }
        public record MergeComponentIdentifier<H, R> : IMergeIdentifier, IComponentIdentifier<MergeSpec<H>, R> where H : ICompositionType where R : class, ResObj
        {
            public IComponentIdentifier<H, R> ForComponent { get; private init; }
            public IComponentIdentifier ForComponentUnsafe => ForComponent;
            public string Source => "CORE";
            public string Identity => $"merge-{ForComponent.Identity}";
            public MergeComponentIdentifier(IComponentIdentifier<H, R> component)
            {
                ForComponent = component;
            }
            public override string ToString() => $"{ForComponent.Identity}*";
        }
    }

    public sealed record Multi<R> : Construct, IMulti<R> where R : class, ResObj
    {
        public IHasElements<R> Container => Values;
        public override IEnumerable<IInstruction> Instructions => Container.Elements.Map(x => x.Instructions).Flatten();
        public required PSequence<R> Values { get; init; } 
        public Updater<PSequence<R>> dValues { init => Values = value(Values); }
        public int Count => Values.Count;
        public bool Equals(Multi<R>? other)
        {
            return other is not null && Container.Elements.SequenceEqual(other.Container.Elements);
        }
        public override int GetHashCode()
        {
            return Container.Elements.GetHashCode();
        }
        public override string ToString()
        {
            return $"[{string.Join(", ", Container.Elements.Map(x => x.ToString()))}]";
        }
    }
}