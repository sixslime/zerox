namespace SixShaded.FourZeroOne.Core.Korssas;

using Roggis;

public record Execute<ROut> : Korssa.Defined.StateImplementedKorssa<MetaFunction<ROut>, ROut>
    where ROut : class, Rog
{
    public Execute(IKorssa<MetaFunction<ROut>> function) : base(function)
    { }

    protected override IOption<FZOSpec.EStateImplemented> MakeData(IKorssaContext _, IOption<MetaFunction<ROut>> funcOpt) => funcOpt.RemapAs(x => x.ConstructMetaExecute());
    protected override IOption<string> CustomToString() => $"!{Arg1}:<>;".AsSome();
}

public record Execute<RArg1, ROut> : Korssa.Defined.StateImplementedKorssa<MetaFunction<RArg1, ROut>, MetaArgs<RArg1>, ROut>
    where RArg1 : class, Rog
    where ROut : class, Rog
{
    public Execute(IKorssa<MetaFunction<RArg1, ROut>> function, IKorssa<MetaArgs<RArg1>> args) : base(function, args)
    { }

    protected override IOption<FZOSpec.EStateImplemented> MakeData(IKorssaContext _, IOption<MetaFunction<RArg1, ROut>> funcOpt, IOption<MetaArgs<RArg1>> argsOpt) =>
        (funcOpt.Check(out var func) && argsOpt.Check(out var args))
            ? func.ConstructMetaExecute(args.Arg1).AsSome()
            : new None<FZOSpec.EStateImplemented>();
    protected override IOption<string> CustomToString() => $"!{Arg1}:{Arg2};".AsSome();
}

public record Execute<RArg1, RArg2, ROut> : Korssa.Defined.StateImplementedKorssa<MetaFunction<RArg1, RArg2, ROut>, MetaArgs<RArg1, RArg2>, ROut>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where ROut : class, Rog
{
    public Execute(IKorssa<MetaFunction<RArg1, RArg2, ROut>> function, IKorssa<MetaArgs<RArg1, RArg2>> args) : base(function, args)
    { }

    protected override IOption<FZOSpec.EStateImplemented> MakeData(IKorssaContext _, IOption<MetaFunction<RArg1, RArg2, ROut>> funcOpt, IOption<MetaArgs<RArg1, RArg2>> argsOpt) =>
        (funcOpt.Check(out var func) && argsOpt.Check(out var args))
            ? func.ConstructMetaExecute(args.Arg1, args.Arg2).AsSome()
            : new None<FZOSpec.EStateImplemented>();
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

    protected override IOption<FZOSpec.EStateImplemented> MakeData(IKorssaContext _, IOption<MetaFunction<RArg1, RArg2, RArg3, ROut>> funcOpt, IOption<MetaArgs<RArg1, RArg2, RArg3>> argsOpt) =>
        (funcOpt.Check(out var func) && argsOpt.Check(out var args))
            ? func.ConstructMetaExecute(args.Arg1, args.Arg2, args.Arg3).AsSome()
            : new None<FZOSpec.EStateImplemented>();
    protected override IOption<string> CustomToString() => $"!{Arg1}:{Arg2};".AsSome();
}