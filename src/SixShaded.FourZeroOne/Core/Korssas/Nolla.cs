namespace SixShaded.FourZeroOne.Core.Korssas;

public sealed record Nolla<R> : Korssa.Defined.Value<R>
    where R : class, Rog
{
    protected override ITask<IOption<R>> Evaluate(IKorssaContext _) => new None<R>().ToCompletedITask();
    protected override IOption<string> CustomToString() => "\u2205".AsSome();
}