namespace SixShaded.FourZeroOne.Core.Korssas.Memory.Object;

public sealed record Remove : Korssa.Defined.PureFunction<IMemoryObject<Rog>, Roggis.Instructions.Redact>
{
    public Remove(IKorssa<IMemoryObject<Rog>> address) : base(address) { }
    protected override Roggis.Instructions.Redact EvaluatePure(IMemoryObject<Rog> in1) => new() { Address = in1 };
    protected override IOption<string> CustomToString() => $"{Arg1} <=X".AsSome();
}