namespace SixShaded.FourZeroOne.Core.Korssas;

using Roggis;

public record Execute<ROut> : Korssa.Defined.StateImplementedKorssa<MetaFunction<ROut>, ROut>
    where ROut : class, Rog
{
    public Execute(IKorssa<MetaFunction<ROut>> function) : base(function)
    { }

    protected override FZOSpec.EStateImplemented MakeData(IKorssaContext _, MetaFunction<ROut> func) => func.ConstructMetaExecute();
    protected override IOption<string> CustomToString() => $"!{Arg1}:<>;".AsSome();
}

public record Execute<RArg1, ROut> : Korssa.Defined.StateImplementedKorssa<MetaFunction<RArg1, ROut>, MetaArgs<RArg1>, ROut>
    where RArg1 : class, Rog
    where ROut : class, Rog
{
    public Execute(IKorssa<MetaFunction<RArg1, ROut>> function, IKorssa<MetaArgs<RArg1>> args) : base(function, args)
    { }

    protected override FZOSpec.EStateImplemented MakeData(IKorssaContext _, MetaFunction<RArg1, ROut> func, MetaArgs<RArg1> args) => func.ConstructMetaExecute(args.Arg1);
    protected override IOption<string> CustomToString() => $"!{Arg1}:{Arg2};".AsSome();
}

public record Execute<RArg1, RArg2, ROut> : Korssa.Defined.StateImplementedKorssa<MetaFunction<RArg1, RArg2, ROut>, MetaArgs<RArg1, RArg2>, ROut>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where ROut : class, Rog
{
    public Execute(IKorssa<MetaFunction<RArg1, RArg2, ROut>> function, IKorssa<MetaArgs<RArg1, RArg2>> args) : base(function, args)
    { }

    protected override FZOSpec.EStateImplemented MakeData(IKorssaContext _, MetaFunction<RArg1, RArg2, ROut> func, MetaArgs<RArg1, RArg2> args) => func.ConstructMetaExecute(args.Arg1, args.Arg2);
    protected override IOption<string> CustomToString() => $"!{Arg1}:{Arg2};".AsSome();
}

public record Execute<RArg1, RArg2, RArg3, ROut> : Korssa.Defined.StateImplementedKorssa<MetaFunction<RArg1, RArg2, RArg3, ROut>, MetaArgs<RArg1, RArg2, RArg3>, ROut>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where RArg3 : class, Rog
    where ROut : class, Rog
{
    public Execute(IKorssa<MetaFunction<RArg1, RArg2, RArg3, ROut>> function, IKorssa<MetaArgs<RArg1, RArg2, RArg3>> args) : base(function, args)
    { }

    protected override FZOSpec.EStateImplemented MakeData(IKorssaContext _, MetaFunction<RArg1, RArg2, RArg3, ROut> func, MetaArgs<RArg1, RArg2, RArg3> args) => func.ConstructMetaExecute(args.Arg1, args.Arg2, args.Arg3);
    protected override IOption<string> CustomToString() => $"!{Arg1}:{Arg2};".AsSome();
}