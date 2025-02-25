namespace SixShaded.FourZeroOne.Core.Tokens.Memory.Object;

public sealed record Remove : Token.Defined.PureFunction<IMemoryObject<Res>, Resolutions.Instructions.Redact>
{
    public Remove(IToken<IMemoryObject<Res>> address) : base(address) { }
    protected override Resolutions.Instructions.Redact EvaluatePure(IMemoryObject<Res> in1) => new() { Address = in1 };
    protected override IOption<string> CustomToString() => $"{Arg1} <=X".AsSome();
}