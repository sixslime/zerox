namespace SixShaded.FourZeroOne.Core.Korssas.Memory;

public sealed record Assign<R>(IRoda<R> address, IKorssa<R> obj) : Korssa.Defined.RegularKorssa<Roggis.Instructions.Assign<R>>(obj)
    where R : class, Rog
{
    public readonly IRoda<R> Address = address;

    protected override ITask<IOption<Roggis.Instructions.Assign<R>>> StandardResolve(IKorssaContext runtime, RogOpt[] args) =>
        new Roggis.Instructions.Assign<R>()
            {
                Address = Address,
                Data = args[0].RemapAs(x => (R)x)
            }.AsSome()
            .ToCompletedITask();

    protected override IOption<string> CustomToString() => $"{Address}<- {ArgKorssas[0]}".AsSome();
}