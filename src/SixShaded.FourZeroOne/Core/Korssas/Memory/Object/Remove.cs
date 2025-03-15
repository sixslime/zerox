namespace SixShaded.FourZeroOne.Core.Korssas.Memory.Object;

using Roveggi;

public sealed record Remove : Korssa.Defined.PureFunction<IRoveggi<IRovedantu<Rog>>, Roggis.Instructions.Redact>
{
    public Remove(IKorssa<IRoveggi<IRovedantu<Rog>>> address) : base(address)
    { }

    protected override Roggis.Instructions.Redact EvaluatePure(IRoveggi<IRovedantu<Rog>> in1) =>
        new()
        {
            Address = in1.MemWrapped(),
        };

    protected override IOption<string> CustomToString() => $"{Arg1} <=X".AsSome();
}