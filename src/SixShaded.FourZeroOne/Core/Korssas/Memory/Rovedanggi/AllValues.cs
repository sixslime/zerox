namespace SixShaded.FourZeroOne.Core.Korssas.Memory.Rovedanggi;

using SixShaded.FourZeroOne.Core.Roggis;
using SixShaded.FourZeroOne.Roveggi;

public sealed record AllValues<D, R> : Korssa.Defined.Value<Multi<R>>
    where D : Rovedantu<R>
    where R : class, Rog
{
    protected override ITask<IOption<Multi<R>>> Evaluate(IKorssaContext runtime) =>
        new Multi<R>(
            runtime.CurrentMemory
                .GetRovedanggiAssignmentsOfType<D, R>()
                .Map(x => x.B.AsSome())).AsSome()
            .ToCompletedITask();
    protected override IOption<string> CustomToString() => $"${typeof(D).Name}(values)".AsSome();

}