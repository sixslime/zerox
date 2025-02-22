namespace SixShaded.FourZeroOne.Core.Tokens.Data;

public sealed record Insert<R> : Token.Defined.PureFunction<IMemoryObject<R>, R, Resolutions.Instructions.Assign<R>>
    where R : class, Res
{
    public Insert(IToken<IMemoryObject<R>> address, IToken<R> obj) : base(address, obj) { }
    protected override Resolutions.Instructions.Assign<R> EvaluatePure(IMemoryObject<R> in1, R in2) => new() { Address = in1, Subject = in2 };
    protected override IOption<string> CustomToString() => $"{Arg1} <== {Arg2}".AsSome();
}