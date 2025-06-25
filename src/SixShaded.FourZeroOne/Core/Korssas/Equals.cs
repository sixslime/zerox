namespace SixShaded.FourZeroOne.Core.Korssas;


public sealed record Equals : Korssa.Defined.PureFunction<Rog, Rog, Roggis.Bool>
{
    public Equals(Kor a, Kor b) : base(a, b)
    { }

    protected override Roggis.Bool EvaluatePure(Rog in1, Rog in2)
    {
        return new()
        {
            IsTrue = in1.Equals(in2)
        };
    }
}