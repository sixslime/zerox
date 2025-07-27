namespace SixShaded.FourZeroOne.Core.Korssas.Bool;

using Roggis;

public sealed record Or : Korssa.Defined.PureFunction<Bool, Bool, Bool>
{
    public Or(IKorssa<Bool> a, IKorssa<Bool> b) : base(a, b)
    { }

    protected override Bool EvaluatePure(Bool a, Bool b) =>
        new()
        {
            IsTrue = a.IsTrue || b.IsTrue
        };

    protected override IOption<string> CustomToString() => $"{Arg1} || {Arg2}".AsSome();
}