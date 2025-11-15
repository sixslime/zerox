namespace SixShaded.FourZeroOne.Core.Korssas.Memory;

public sealed record Reference<R>() : Korssa.Defined.Value<R>
    where R : class, Rog
{
    public required IRoda<R> Roda { get; init; }

    protected override ITask<IOption<R>> Evaluate(IKorssaContext runtime) => runtime.CurrentMemory.GetObject(Roda).ToCompletedITask();
    protected override IOption<string> CustomToString() => $"&{Roda}".AsSome();
}