#nullable enable
namespace SixShaded.FourZeroOne.Macro
{
    using Core.Resolutions.Boxed;
    using FourZeroOne.FZOSpec;
    using SixShaded.NotRust;
    using Token;
    using Res = Resolution.Res;

    public record Macro<RVal> : RuntimeHandledValue<RVal>, IMacro<RVal>, IMacroValue<RVal>
        where RVal : Res
    {
        public Macro() : base() { }
        public object[] CustomData { get; init; } = [];
        public required MacroLabel Label { get; init; }
        public required MetaFunction<RVal> Definition { get; init; }
        protected override EStateImplemented MakeData()
        {
            return Definition.GenerateMetaExecute();
        }
        protected override IOption<string> CustomToString()
            => $"{Label.Package}.{Label.Identifier}()".AsSome();
    }
    public record Macro<RArg1, ROut> : RuntimeHandledFunction<RArg1, ROut>, IMacroFunction<RArg1, ROut>
        where RArg1 : Res
        where ROut : Res
    {
        public Macro(IToken<RArg1> in1) : base(in1) { }
        public required MacroLabel Label { get; init; }
        public object[] CustomData { get; init; } = [];
        public required MetaFunction<RArg1, ROut> Definition { get; init; }
        protected override EStateImplemented MakeData(RArg1 in1)
        {
            return Definition.GenerateMetaExecute(in1.AsSome());
        }
        protected override IOption<string> CustomToString()
            => $"{Label.Package}.{Label.Identifier}({Arg1})".AsSome();
    }
    public record Macro<RArg1, RArg2, ROut> : RuntimeHandledFunction<RArg1, RArg2, ROut>, IMacroFunction<RArg1, RArg2, ROut>
        where RArg1 : Res
        where RArg2 : Res
        where ROut : Res
    {
        public Macro(IToken<RArg1> in1, IToken<RArg2> in2) : base(in1, in2) { }
        public required MacroLabel Label { get; init; }
        public object[] CustomData { get; init; } = [];
        public required MetaFunction<RArg1, RArg2, ROut> Definition { get; init; }
        protected override EStateImplemented MakeData(RArg1 in1, RArg2 in2)
        {
            return Definition.GenerateMetaExecute(in1.AsSome(), in2.AsSome());
        }
        protected override IOption<string> CustomToString()
            => $"{Label.Package}.{Label.Identifier}({Arg1}, {Arg2})".AsSome();
    }
    public record Macro<RArg1, RArg2, RArg3, ROut> : RuntimeHandledFunction<RArg1, RArg2, RArg3, ROut>, IMacroFunction<RArg1, RArg2, RArg3, ROut>
        where RArg1 : Res
        where RArg2 : Res
        where RArg3 : Res
        where ROut : Res
    {
        public Macro(IToken<RArg1> in1, IToken<RArg2> in2, IToken<RArg3> in3) : base(in1, in2, in3) { }
        public required MacroLabel Label { get; init; }
        public object[] CustomData { get; init; } = [];
        public required MetaFunction<RArg1, RArg2, RArg3, ROut> Definition { get; init; }
        protected override EStateImplemented MakeData(RArg1 in1, RArg2 in2, RArg3 in3)
        {
            return Definition.GenerateMetaExecute(in1.AsSome(), in2.AsSome(), in3.AsSome());
        }
        protected override IOption<string> CustomToString()
            => $"{Label.Package}.{Label.Identifier}({Arg1}, {Arg2}, {Arg3})".AsSome();
    }
    
    
    
}