namespace SixShaded.FourZeroOne.Core.Korssas.Memory;

public sealed record Assign<R>(IKorssa<R> obj) : Korssa.Defined.RegularKorssa<Roggis.Instructions.Assign<R>>(obj)
    where R : class, Rog
{
    public required IRoda<R> Roda { get; init; }

    protected override ITask<IOption<Roggis.Instructions.Assign<R>>> StandardResolve(IKorssaContext runtime, RogOpt[] args) =>
        new Roggis.Instructions.Assign<R>()
            {
                Roda = Roda,
                Data = args[0].RemapAs(x => (R)x)
            }.AsSome()
            .ToCompletedITask();

    protected override IOption<string> CustomToString() => $"{Roda}<- {ArgKorssas[0]}".AsSome();
}