namespace SixShaded.FourZeroOne.Core.Korssas.Memory.Rovedanggi;

using SixShaded.FourZeroOne.Core.Roggis;
using SixShaded.FourZeroOne.Roveggi;

public sealed record AllKeys<D, R> : Korssa.Defined.Value<Multi<IRoveggi<D>>>
    where D : Rovedantu<R>
    where R : class, Rog
{
    protected override ITask<IOption<Multi<IRoveggi<D>>>> Evaluate(IKorssaContext runtime) =>
        new Multi<IRoveggi<D>>(
            runtime.CurrentMemory
                .GetRovedanggiAssignmentsOfType<D, R>()
                .Map(x => x.A.AsSome())).AsSome()
            .ToCompletedITask();
    protected override IOption<string> CustomToString() => $"${typeof(D).Name}(keys)".AsSome();
}