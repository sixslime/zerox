#nullable enable
using FourZeroOne;

namespace SixShaded.FourZeroOne.Core.Tokens
{
    public record Execute<ROut> : RuntimeHandledFunction<r.Boxed.MetaFunction<ROut>, ROut>
        where ROut : class, Res
    {
        public Execute(IToken<r.Boxed.MetaFunction<ROut>> function) : base(function) { }

        protected override FZOSpec.EStateImplemented MakeData(r.Boxed.MetaFunction<ROut> func)
        {
            return func.GenerateMetaExecute();
        }
        protected override IOption<string> CustomToString() => $"!{Arg1}:<>;".AsSome();
    }
    public record Execute<RArg1, ROut> : RuntimeHandledFunction<r.Boxed.MetaFunction<RArg1, ROut>, r.Boxed.MetaArgs<RArg1>, ROut>
        where RArg1 : class, Res
        where ROut : class, Res
    {
        public Execute(IToken<r.Boxed.MetaFunction<RArg1, ROut>> function, IToken<r.Boxed.MetaArgs<RArg1>> args) : base(function, args) { }

        protected override FZOSpec.EStateImplemented MakeData(r.Boxed.MetaFunction<RArg1, ROut> func, r.Boxed.MetaArgs<RArg1> args)
        {
            return func.GenerateMetaExecute(args.Arg1);
        }
        protected override IOption<string> CustomToString() => $"!{Arg1}:{Arg2};".AsSome();
    }
    public record Execute<RArg1, RArg2, ROut> : RuntimeHandledFunction<r.Boxed.MetaFunction<RArg1, RArg2, ROut>, r.Boxed.MetaArgs<RArg1, RArg2>, ROut>
        where RArg1 : class, Res
        where RArg2 : class, Res
        where ROut : class, Res
    {
        public Execute(IToken<r.Boxed.MetaFunction<RArg1, RArg2, ROut>> function, IToken<r.Boxed.MetaArgs<RArg1, RArg2>> args) : base(function, args) { }

        protected override FZOSpec.EStateImplemented MakeData(r.Boxed.MetaFunction<RArg1, RArg2, ROut> func, r.Boxed.MetaArgs<RArg1, RArg2> args)
        {
            return func.GenerateMetaExecute(args.Arg1, args.Arg2);
        }
        protected override IOption<string> CustomToString() => $"!{Arg1}:{Arg2};".AsSome();
    }
    public record Execute<RArg1, RArg2, RArg3, ROut> : RuntimeHandledFunction<r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut>, r.Boxed.MetaArgs<RArg1, RArg2, RArg3>, ROut>
        where RArg1 : class, Res
        where RArg2 : class, Res
        where RArg3 : class, Res
        where ROut : class, Res
    {
        public Execute(IToken<r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut>> function, IToken<r.Boxed.MetaArgs<RArg1, RArg2, RArg3>> args) : base(function, args) { }

        protected override FZOSpec.EStateImplemented MakeData(r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut> func, r.Boxed.MetaArgs<RArg1, RArg2, RArg3> args)
        {
            return func.GenerateMetaExecute(args.Arg1, args.Arg2, args.Arg3);
        }
        protected override IOption<string> CustomToString() => $"!{Arg1}:{Arg2};".AsSome();
    }
}