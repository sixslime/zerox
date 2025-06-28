namespace SixShaded.FourZeroOne.Core.Korssas.Memory.Object;

using Roveggi;

public sealed record Remove : Korssa.Defined.PureFunction<IRoveggi<Rovedantu<Rog>>, Roggis.Instructions.Redact>
{
    public Remove(IKorssa<IRoveggi<Rovedantu<Rog>>> address) : base(address)
    { }

    protected override Roggis.Instructions.Redact EvaluatePure(IRoveggi<Rovedantu<Rog>> in1) =>
        new()
        {
            Address = in1.Rovedanggi(),
        };

    protected override IOption<string> CustomToString() => $"{Arg1} <=X".AsSome();
}