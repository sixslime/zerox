namespace SixShaded.FourZeroOne.Core.Korssas.Memory;

public sealed record DynamicAssign<R> : Korssa.Defined.RegularKorssa<Roggis.Instructions.Assign<R>>
    where R : class, Rog
{
    public readonly IMemoryAddress<R> AssigningAddress;

    public DynamicAssign(IMemoryAddress<R> address, IKorssa<R> obj) : base(obj)
    {
        AssigningAddress = address;
    }

    protected override ITask<IOption<Roggis.Instructions.Assign<R>>> StandardResolve(IKorssaContext runtime, RogOpt[] args) =>
        args[0]
            .RemapAs(
            x =>
                new Roggis.Instructions.Assign<R>
                {
                    Address = AssigningAddress,
                    Subject = (R)x,
                })
            .ToCompletedITask();

    protected override IOption<string> CustomToString() => $"{AssigningAddress}<- {ArgKorssas[0]}".AsSome();
}