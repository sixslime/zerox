namespace SixShaded.FourZeroOne.Core.Tokens;

public sealed record DynamicAssign<R> : Token.Defined.StandardToken<Resolutions.Instructions.Assign<R>> where R : class, Res
{
    public readonly IMemoryAddress<R> AssigningAddress;

    public DynamicAssign(IMemoryAddress<R> address, IToken<R> obj) : base(obj)
    {
        AssigningAddress = address;
    }

    protected override ITask<IOption<Resolutions.Instructions.Assign<R>>> StandardResolve(ITokenContext runtime, IOption<Res>[] args) => args[0].RemapAs(x => new Resolutions.Instructions.Assign<R> { Address = AssigningAddress, Subject = (R)x }).ToCompletedITask();
    protected override IOption<string> CustomToString() => $"{AssigningAddress}<- {ArgTokens[0]}".AsSome();
}