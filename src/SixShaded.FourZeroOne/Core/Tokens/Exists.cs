namespace SixShaded.FourZeroOne.Core.Tokens;

public sealed record Exists : Token.Defined.Function<Res, Resolutions.Bool>
{
    public Exists(Tok obj) : base(obj) { }
    protected override ITask<IOption<Resolutions.Bool>> Evaluate(ITokenContext _, IOption<Res> obj) => new Resolutions.Bool { IsTrue = obj.IsSome() }.AsSome().ToCompletedITask();
}