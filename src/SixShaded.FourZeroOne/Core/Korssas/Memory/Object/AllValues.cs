namespace SixShaded.FourZeroOne.Core.Korssas.Memory.Object;

using Roveggi;
using Roggis;

public sealed record AllValues<D, R> : Korssa.Defined.Value<Multi<R>>
    where D : Rovedantu<R>
    where R : class, Rog
{
    protected override ITask<IOption<Multi<R>>> Evaluate(IKorssaContext runtime) =>
        new Multi<R>
            {
                Values =
                    runtime.CurrentMemory
                        .GetRovedanggiAssignmentsOfType<D, R>()
                        .Map(x => x.B)
                        .ToPSequence(),
            }.AsSome()
            .ToCompletedITask();
    protected override IOption<string> CustomToString() => $"${typeof(D).Name}(values)".AsSome();

}