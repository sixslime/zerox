
using System.Collections.Generic;
using Perfection;
using ControlledFlows;
using MorseCode.ITask;
#nullable enable
namespace FourZeroOne.Macro
{
    using FourZeroOne.FZOSpec;
    using Token;
    using ResObj = Resolution.IResolution;
    using Core.Resolutions.Boxed;

    public interface IMacro<R> : Unsafe.IMacro, IToken<R> where R : class, ResObj { }
    public record Macro<RArg1, RArg2, RArg3, ROut> : RuntimeHandledFunction<RArg1, RArg2, RArg3, ROut>, IMacro<ROut>
        where RArg1 : class, ResObj
        where RArg2 : class, ResObj
        where RArg3 : class, ResObj
        where ROut : class, ResObj
    {
        public Macro(IToken<RArg1> in1, IToken<RArg2> in2, IToken<RArg3> in3) : base(in1, in2, in3) { }
        public required MacroLabel Label { get; init; }
        public required MetaFunction<RArg1, RArg2, RArg3, ROut> Definition { get; init; }
        protected override EStateImplemented MakeData(RArg1 in1, RArg2 in2, RArg3 in3)
        {
            return Definition.GenerateMetaExecute(in1.AsSome(), in2.AsSome(), in3.AsSome());
        }
    }
    public record Macro<RArg1, RArg2, ROut> : RuntimeHandledFunction<RArg1, RArg2, ROut>, IMacro<ROut>
        where RArg1 : class, ResObj
        where RArg2 : class, ResObj
        where ROut : class, ResObj
    {
        public Macro(IToken<RArg1> in1, IToken<RArg2> in2) : base(in1, in2) { }
        public required MacroLabel Label { get; init; }
        public required MetaFunction<RArg1, RArg2, ROut> Definition { get; init; }
        protected override EStateImplemented MakeData(RArg1 in1, RArg2 in2)
        {
            return Definition.GenerateMetaExecute(in1.AsSome(), in2.AsSome());
        }
    }
    public record Macro<RArg1, ROut> : RuntimeHandledFunction<RArg1, ROut>, IMacro<ROut>
        where RArg1 : class, ResObj
        where ROut : class, ResObj
    {
        public Macro(IToken<RArg1> in1) : base(in1) { }
        public required MacroLabel Label { get; init; }
        public required MetaFunction<RArg1, ROut> Definition { get; init; }
        protected override EStateImplemented MakeData(RArg1 in1)
        {
            return Definition.GenerateMetaExecute(in1.AsSome());
        }
    }
    public record Macro<ROut> : RuntimeHandledValue<ROut>, IMacro<ROut>
        where ROut : class, ResObj
    {
        public Macro() : base() { }
        public required MacroLabel Label { get; init; }
        public required MetaFunction<ROut> Definition { get; init; }
        protected override EStateImplemented MakeData()
        {
            return Definition.GenerateMetaExecute();
        }
    }
    public sealed record MacroLabel
    {
        public required string Namespace { get; init; }
        public required string Identifier { get; init; }
    }
    namespace Unsafe
    {
        public interface IMacro : Token.Unsafe.IToken
        {
            public MacroLabel Label { get; }
        }
    }
}