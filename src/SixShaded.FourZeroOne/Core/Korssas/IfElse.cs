namespace SixShaded.FourZeroOne.Core.Korssas;

using Roggis;

public record TryCast<R> : Korssa.Defined.Function<Rog, R>
    where R : class, Rog
{
    public TryCast(IKorssa<Rog> obj) : base(obj)
    { }

    protected override ITask<IOption<R>> Evaluate(IKorssaContext _, RogOpt in1) => in1.RemapAs(x => x.MightBeA<R>()).Press().ToCompletedITask();
    protected override IOption<string> CustomToString() => $"{Arg1}%({typeof(R).Name})".AsSome();
}