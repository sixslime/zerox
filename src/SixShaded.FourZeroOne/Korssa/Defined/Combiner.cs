﻿namespace SixShaded.FourZeroOne.Korssa.Defined;

public abstract record Combiner<RArg, ROut> : RegularKorssa<ROut>, IHasCombinerArgs<RArg, ROut>
    where RArg : class, Rog
    where ROut : class, Rog
{
    protected Combiner(params IKorssa<RArg>[] korssas) : base(korssas.IEnumerable())
    { }
    protected Combiner(IEnumerable<IKorssa<RArg>> korssas) : base(korssas)
    { }
    protected sealed override ITask<IOption<ROut>> StandardResolve(IKorssaContext runtime, RogOpt[] korssas) => Evaluate(runtime, korssas.Map(x => x.RemapAs(x => (RArg)x)));
    protected abstract ITask<IOption<ROut>> Evaluate(IKorssaContext runtime, IEnumerable<IOption<RArg>> inputs);
    public IEnumerable<IKorssa<RArg>> Args => ArgKorssas.Map(x => (IKorssa<RArg>)x);
}