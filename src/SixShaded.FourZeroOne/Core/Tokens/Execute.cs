#nullable enable
namespace SixShaded.FourZeroOne.Core.Tokens
{
    
    public record Execute<ROut> : Token.Defined.RuntimeHandledFunction<MetaFunction<ROut>, ROut>
        where ROut : class, Res
    {
        public Execute(IToken<MetaFunction<ROut>> function) : base(function) { }

        protected override FZOSpec.EStateImplemented MakeData(MetaFunction<ROut> func)
        {
            return func.GenerateMetaExecute();
        }
        protected override IOption<string> CustomToString() => $"!{Arg1}:<>;".AsSome();
    }
    public record Execute<RArg1, ROut> : Token.Defined.RuntimeHandledFunction<MetaFunction<RArg1, ROut>, MetaArgs<RArg1>, ROut>
        where RArg1 : class, Res
        where ROut : class, Res
    {
        public Execute(IToken<MetaFunction<RArg1, ROut>> function, IToken<MetaArgs<RArg1>> args) : base(function, args) { }

        protected override FZOSpec.EStateImplemented MakeData(MetaFunction<RArg1, ROut> func, MetaArgs<RArg1> args)
        {
            return func.GenerateMetaExecute(args.Arg1);
        }
        protected override IOption<string> CustomToString() => $"!{Arg1}:{Arg2};".AsSome();
    }
    public record Execute<RArg1, RArg2, ROut> : Token.Defined.RuntimeHandledFunction<MetaFunction<RArg1, RArg2, ROut>, MetaArgs<RArg1, RArg2>, ROut>
        where RArg1 : class, Res
        where RArg2 : class, Res
        where ROut : class, Res
    {
        public Execute(IToken<MetaFunction<RArg1, RArg2, ROut>> function, IToken<MetaArgs<RArg1, RArg2>> args) : base(function, args) { }

        protected override FZOSpec.EStateImplemented MakeData(MetaFunction<RArg1, RArg2, ROut> func, MetaArgs<RArg1, RArg2> args)
        {
            return func.GenerateMetaExecute(args.Arg1, args.Arg2);
        }
        protected override IOption<string> CustomToString() => $"!{Arg1}:{Arg2};".AsSome();
    }
    public record Execute<RArg1, RArg2, RArg3, ROut> : Token.Defined.RuntimeHandledFunction<MetaFunction<RArg1, RArg2, RArg3, ROut>, MetaArgs<RArg1, RArg2, RArg3>, ROut>
        where RArg1 : class, Res
        where RArg2 : class, Res
        where RArg3 : class, Res
        where ROut : class, Res
    {
        public Execute(IToken<MetaFunction<RArg1, RArg2, RArg3, ROut>> function, IToken<MetaArgs<RArg1, RArg2, RArg3>> args) : base(function, args) { }

        protected override FZOSpec.EStateImplemented MakeData(MetaFunction<RArg1, RArg2, RArg3, ROut> func, MetaArgs<RArg1, RArg2, RArg3> args)
        {
            return func.GenerateMetaExecute(args.Arg1, args.Arg2, args.Arg3);
        }
        protected override IOption<string> CustomToString() => $"!{Arg1}:{Arg2};".AsSome();
    }
}