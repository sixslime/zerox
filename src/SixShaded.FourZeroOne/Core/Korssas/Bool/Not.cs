namespace SixShaded.FourZeroOne.Core.Korssas.Bool;

using Roggis;

public sealed record Not : Korssa.Defined.PureFunction<Bool, Bool>
{
    public Not(IKorssa<Bool> val) : base(val)
    { }

    protected override Bool EvaluatePure(Bool val) =>
        new()
        {
            IsTrue = !val.IsTrue
        };

    protected override IOption<string> CustomToString() => $"!{Arg1}".AsSome();
}