namespace SixShaded.FourZeroOne.Core.Korssas.Memory;

using Roveggi;

public sealed record Remove : Korssa.Defined.PureFunction<IRoveggi<IMemoryRovetu<Rog>>, Roggis.Instructions.Redact>
{
    public Remove(IKorssa<IRoveggi<IMemoryRovetu<Rog>>> address) : base(address)
    { }

    protected override Roggis.Instructions.Redact EvaluatePure(IRoveggi<IMemoryRovetu<Rog>> in1) =>
        new()
        {
            Address = in1.MemWrapped(),
        };

    protected override IOption<string> CustomToString() => $"{Arg1} <=X".AsSome();
}