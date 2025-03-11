namespace SixShaded.FourZeroOne.Core.Korssas;

public sealed record Exists : Korssa.Defined.Function<Rog, Roggis.Bool>
{
    public Exists(Kor obj) : base(obj)
    { }

    protected override ITask<IOption<Roggis.Bool>> Evaluate(IKorssaContext _, RogOpt obj) =>
        new Roggis.Bool
            {
                IsTrue = obj.IsSome(),
            }.AsSome()
            .ToCompletedITask();
}